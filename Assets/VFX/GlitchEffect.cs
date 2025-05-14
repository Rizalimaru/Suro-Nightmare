using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class GlitchEffectSprite : MonoBehaviour
{
    public Texture2D displacementMap;
    public Shader glitchShader;

    [Header("Glitch Intensity")]
    [Range(0, 1)] public float intensity;
    [Range(0, 1)] public float flipIntensity;
    [Range(0, 1)] public float colorIntensity;

    private Material _material;
    private float _glitchup;
    private float _glitchdown;
    private float flicker;
    private float _glitchupTime = 0.05f;
    private float _glitchdownTime = 0.05f;
    private float _flickerTime = 0.5f;



    void Start()
    {
        // Buat material dari shader glitch
        if (glitchShader != null)
        {
            _material = new Material(glitchShader);
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.material = _material; // Terapkan material ke sprite
        }
    }

    void Update()
    {
        if (_material == null) return;

        // Set parameter glitch
        _material.SetFloat("_Intensity", intensity);
        _material.SetFloat("_ColorIntensity", colorIntensity);
        _material.SetTexture("_DispTex", displacementMap);

        // Flicker effect
        flicker += Time.deltaTime * colorIntensity;
        if (flicker > _flickerTime)
        {
            _material.SetFloat("filterRadius", Random.Range(-3f, 3f) * colorIntensity);
            _material.SetVector("direction", Quaternion.AngleAxis(Random.Range(0, 360) * colorIntensity, Vector3.forward) * Vector4.one);
            flicker = 0;
            _flickerTime = Random.value;
        }

        if (colorIntensity == 0)
            _material.SetFloat("filterRadius", 0);

        // Flip up effect
        _glitchup += Time.deltaTime * flipIntensity;
        if (_glitchup > _glitchupTime)
        {
            if (Random.value < 0.1f * flipIntensity)
                _material.SetFloat("flip_up", Random.Range(0, 1f) * flipIntensity);
            else
                _material.SetFloat("flip_up", 0);

            _glitchup = 0;
            _glitchupTime = Random.value / 10f;
        }

        if (flipIntensity == 0)
            _material.SetFloat("flip_up", 0);

        // Flip down effect
        _glitchdown += Time.deltaTime * flipIntensity;
        if (_glitchdown > _glitchdownTime)
        {
            if (Random.value < 0.1f * flipIntensity)
                _material.SetFloat("flip_down", 1 - Random.Range(0, 1f) * flipIntensity);
            else
                _material.SetFloat("flip_down", 1);

            _glitchdown = 0;
            _glitchdownTime = Random.value / 10f;
        }

        if (flipIntensity == 0)
            _material.SetFloat("flip_down", 1);

        // Displacement effect
        if (Random.value < 0.05 * intensity)
        {
            _material.SetFloat("displace", Random.value * intensity);
            _material.SetFloat("scale", 1 - Random.value * intensity);
        }
        else
            _material.SetFloat("displace", 0);
    }


    public void TriggerGlitch(float duration)
    {
        StartCoroutine(AnimateGlitch(duration));
    }

    private IEnumerator AnimateGlitch(float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // Atur parameter glitch secara acak
            intensity = Random.Range(0.2f, 0.8f);
            flipIntensity = Random.Range(0.2f, 0.8f);
            colorIntensity = Random.Range(0.2f, 0.8f);

            yield return new WaitForSeconds(0.1f); // Ubah parameter setiap 0.1 detik
        }

        // Reset parameter glitch ke 0 setelah animasi selesai
        intensity = 0f;
        flipIntensity = 0f;
        colorIntensity = 0f;
    }
}
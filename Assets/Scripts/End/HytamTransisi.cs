using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HytamTransisi : MonoBehaviour
{

        public Volume postProcessVolume;
    private ChromaticAberration chromatic;
    private FilmGrain filmGrain;
    private DepthOfField depthOfField;

    public float glitchDuration = 10f;
    public SceneController sceneController;
    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    private void OnEnable()
    {
        AudioManager.Instance.PlaySFX("End", 1); // Memutar suara Hytam
                // Ambil komponen efek dari volume
        if (postProcessVolume.profile.TryGet(out chromatic) &&
            postProcessVolume.profile.TryGet(out filmGrain) &&
            postProcessVolume.profile.TryGet(out depthOfField))
        {
            DisableEffects(); // Mulai dari keadaan mati
            StartCoroutine(ActivateGlitch());
        }
        StartCoroutine(WaitForAnimationAndLoadScene());
    }

    private IEnumerator WaitForAnimationAndLoadScene()
    {


        // Tunggu hingga animasi selesai
        yield return new WaitForSeconds(12f);
        AudioManager.Instance.StopBackgroundMusicWithTransition("End", 1f); // Hentikan musik latar dengan transisi

        // Setelah animasi selesai, muat scene baru
        sceneController.LoadScene("Mainmenu");
    }

      void DisableEffects()
    {
        chromatic.intensity.Override(0f);
        filmGrain.intensity.Override(0f);
        depthOfField.active = false;
    }

    IEnumerator ActivateGlitch()
    {
        float timer = 0f;

        // Aktifkan efek secara dinamis
        chromatic.intensity.Override(1f);
        filmGrain.intensity.Override(1f);
        depthOfField.active = true;
        depthOfField.focusDistance.Override(0.1f); // Fokus dekat, efek kabur

        while (timer < glitchDuration)
        {
            // (Opsional) tambahkan variasi acak untuk efek glitch:
            chromatic.intensity.value = Random.Range(0.8f, 1f);
            filmGrain.intensity.value = Random.Range(0.7f, 1f);
            depthOfField.focusDistance.value = Random.Range(0.05f, 0.2f);

            timer += Time.deltaTime;
            yield return null;
        }

        // Kembalikan ke keadaan semula
        DisableEffects();
    }
}

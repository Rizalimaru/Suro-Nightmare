using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class KuntilanakSound : MonoBehaviour
{
    public Transform player;
    public AudioSource heartbeatAudio;
    public float maxDistance = 10f;
    public float minPitch = 0.8f;
    public float maxPitch = 2.0f;

    public Volume globalVolume; // Drag Global Volume di Inspector
    private Vignette vignette; // Referensi ke efek vignette


    private SembunyiLemari sembunyiScript;

    void Start()
    {
        if (globalVolume != null && globalVolume.profile != null)
        {
            globalVolume.profile.TryGet(out vignette);
        }

        if (player != null)
        {
            sembunyiScript = player.GetComponentInChildren<SembunyiLemari>(); // kalau script sembunyi ada di anak
            if (sembunyiScript == null)
                sembunyiScript = player.GetComponent<SembunyiLemari>(); // fallback
        }
    }

    void Update()
    {
        if (player == null || heartbeatAudio == null || vignette == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < maxDistance)
        {
            if (!heartbeatAudio.isPlaying)
                heartbeatAudio.Play();

            float t = 1 - (distance / maxDistance);
            heartbeatAudio.pitch = Mathf.Lerp(minPitch, maxPitch, t);

            // Ubah intensitas vignette (misal makin gelap makin dekat)
            vignette.intensity.value = Mathf.Lerp(0f, 0.85f, t);
        }
        else
        {
            // Redupkan vignette saat jauh
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0f, Time.deltaTime * 2f);
        }
    }


    private bool sembunyiScriptIsHiding()
    {
        return sembunyiScript != null && (bool)sembunyiScript.GetType().GetField("isHiding", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(sembunyiScript);
    }
}

using UnityEngine;

public class KuntilanakSound : MonoBehaviour
{
    public Transform player;
    public AudioSource heartbeatAudio;
    public float maxDistance = 10f;
    public float minPitch = 0.8f;
    public float maxPitch = 2.0f;

    private SembunyiLemari sembunyiScript;

    void Start()
    {
        if (player != null)
        {
            sembunyiScript = player.GetComponentInChildren<SembunyiLemari>(); // kalau script sembunyi ada di anak
            if (sembunyiScript == null)
                sembunyiScript = player.GetComponent<SembunyiLemari>(); // fallback
        }
    }

    void Update()
    {
        if (player == null || heartbeatAudio == null) return;


        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < maxDistance)
        {
            if (!heartbeatAudio.isPlaying)
                heartbeatAudio.Play();

            float t = 1 - (distance / maxDistance);
            heartbeatAudio.pitch = Mathf.Lerp(minPitch, maxPitch, t);
        }

    }

    private bool sembunyiScriptIsHiding()
    {
        return sembunyiScript != null && (bool)sembunyiScript.GetType().GetField("isHiding", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(sembunyiScript);
    }
}

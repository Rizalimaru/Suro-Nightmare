using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class kerisEffect : MonoBehaviour
{
    public KeyCode stunKey = KeyCode.F;
    public float stunRadius = 5f;
    public float stunDuration = 3f;
    public Light2D flashbangLight; // Referensi ke Light2D untuk flashbang

    private Animator anim;
    void Start()
    {
        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    void Update()
    {
        // Cek apakah pemain sedang berinteraksi
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<playerController>().isInteracting)
        {
            return; // Jangan lakukan apa-apa jika pemain sedang berinteraksi
        }

        if (Input.GetKeyDown(stunKey))
        {
            StartCoroutine(TriggerStunWithAnimation());
        }
    }

    private IEnumerator TriggerStunWithAnimation()
    {   
        TriggerStun();
        // Nonaktifkan gerakan pemain
        playerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<playerController>();
        player.canMove = false;
        anim.SetBool("isWalking", false); // Set animator ke idle

        // Mainkan animasi Keris
        anim.SetTrigger("Keris");

        // Tingkatkan intensitas flashbangLight secara bertahap
        yield return StartCoroutine(ChangeLightIntensity(flashbangLight, 30f, 0.5f)); // Naik ke 30 dalam 0.5 detik

        // Tunggu hingga animasi selesai (sesuaikan durasi dengan animasi Anda)
        yield return new WaitForSeconds(2f);

        // Turunkan intensitas flashbangLight secara bertahap
        yield return StartCoroutine(ChangeLightIntensity(flashbangLight, 0f, 0.5f)); // Turun ke 0 dalam 0.5 detik

        // Aktifkan kembali gerakan pemain
        anim.SetBool("isWalking", true); // Set animator ke idle
        player.canMove = true;

        
    }

    private IEnumerator ChangeLightIntensity(Light2D light, float targetIntensity, float duration)
    {
        float startIntensity = light.intensity;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            light.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / duration);
            yield return null; // Tunggu frame berikutnya
        }

        light.intensity = targetIntensity; // Pastikan nilai akhir sesuai target
    }

    public void TriggerStun()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, stunRadius);

        foreach (Collider2D col in hitObjects)
        {
            if (col.CompareTag("Pocong"))
            {
                // Memanggil metode Stun dari enemyPocong untuk memberikan efek stun
                enemyPocong ai = col.GetComponent<enemyPocong>();
                if (ai != null)
                {
                    ai.Stun(stunDuration);  // Memberikan efek stun pada pocong
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, stunRadius);
    }
}

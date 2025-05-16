using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class kerisEffect : MonoBehaviour
{
    public KeyCode stunKey = KeyCode.E;
    public float stunRadius = 5f;
    public float stunDuration = 3f;
    public Light2D flashbangLight; // Referensi ke Light2D untuk flashbang
    private playerController playerController; // Referensi ke playerController

    private bool isUseKeris = false;

    private Animator anim;
    void Start()
    {
        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<playerController>();
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
        // Cek apakah pemain sedang berinteraksi
        if (playerController.isInteracting)
        {
            return; // Jangan lakukan apa-apa jika pemain sedang berinteraksi
        }

        // Cek apakah tombol stun ditekan dan stun tidak sedang berlangsung
        if (Input.GetKeyDown(stunKey) && !isUseKeris && playerController.isGrounded)
        {
            StartCoroutine(TriggerStunWithAnimation());
        }
        }
    }

    private IEnumerator TriggerStunWithAnimation()
    {   
        isUseKeris = true;
        AudioManager.Instance.PlaySFX("Stage2", 0);
        TriggerStun();
        AudioManager.Instance.StopSFX("PlayerMovement", 0);
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


        yield return new WaitUntil(() => Input.GetAxis("Horizontal") != 0);
        AudioManager.Instance.PlaySFX("PlayerMovement", 0); // Play SFX default
        // Aktifkan kembali gerakan pemain
        anim.SetBool("isWalking", true); // Set animator ke idle
        player.canMove = true;

        isUseKeris = false; 

        
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

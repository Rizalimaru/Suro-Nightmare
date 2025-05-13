using UnityEngine;
using TMPro;
using JetBrains.Annotations;
using System.Collections;

public class SembunyiLemari : MonoBehaviour
{
    private bool canHide = false;
    public bool isHiding = false;
    private GameObject player;

    public bool IsHiding => isHiding;

    public TextMeshProUGUI hideTextUI;

    public BoxCollider2D [] kuntilanakCollider;

    public GameObject lemari2d;

    public GameObject lampu2d;

    // mendeteksi biar tidak bisa di spam
    private bool isHidingCoroutineRunning = false;

    void Start()
    {
        if (hideTextUI != null)
            hideTextUI.gameObject.SetActive(false);
    }

    void Update()
    {
        if (canHide && Input.GetKeyDown(KeyCode.E) && isHidingCoroutineRunning == false)
        {
            isHidingCoroutineRunning = true;


            isHiding = !isHiding;

            // Jalankan coroutine untuk mengatur jeda
            StartCoroutine(HandleHiding(isHiding));
        }
    }

    private IEnumerator HandleHiding(bool hide)
    {
        var rb = player.GetComponent<Rigidbody2D>();
        var controller = player.GetComponent<playerController>();

        var playerAnim = player.GetComponent<Animator>();

        // Stop semua animasi player
        playerAnim.SetBool("isWalking", false);

        controller.enabled = false; // Nonaktifkan kontrol player saat bersembunyi

        
        // Tampilkan lemari 2D terlebih dahulu
        lemari2d.SetActive(true);


        foreach (BoxCollider2D collider in kuntilanakCollider)
        {
            if (collider != null) collider.enabled = !hide;
        }

        // Tunggu selama 1 detik
        yield return new WaitForSeconds(1f);

        

        controller.enabled = !hide;

        isHidingCoroutineRunning = false;
        lemari2d.SetActive(false); // Sembunyikan lemari 2D setelah jeda

        // Sembunyikan atau tampilkan player dan kontrol

        //matikan sprite renderer player
        var spriteRenderer = player.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = !hide;
        }

        lampu2d.SetActive(!hide);

 

        if (hide)
        {
            rb.velocity = Vector2.zero;
        }

        // Jika keluar dari lemari, sembunyikan lemari 2D
        if (!hide)
        {
            lemari2d.SetActive(false);
        }

        // Perbarui teks UI
        UpdateHideText();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canHide = true;
            player = other.gameObject;

            if (hideTextUI != null)
            {
                hideTextUI.gameObject.SetActive(true);
                UpdateHideText();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canHide = false;

            if (hideTextUI != null)
                hideTextUI.gameObject.SetActive(false);
        }
    }

    void UpdateHideText()
    {
        if (hideTextUI == null) return;

        if (isHiding)
            hideTextUI.text = "Tekan E untuk keluar";
        else
            hideTextUI.text = "Tekan E untuk bersembunyi";
    }
    
}

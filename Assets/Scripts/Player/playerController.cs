using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
public class playerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public bool isCrounching = false;
    public bool canMove = true; // Variabel untuk mengontrol gerakan
    public bool isInteracting = false; // Menyimpan status interaksi pemain
    private Animator animator; // Tambahkan variabel animator
    private SpriteRenderer spriteRenderer; // Tambahkan referensi ke SpriteRenderer
    public GameObject lampu;
    public PlayerItemData playerItemData;
    public Volume globalVolume; // Drag Global Volume di Inspector
    private Vignette vignette; // Referensi ke efek vignette


    [Header("Audio Player")]   
    public GameObject jumpSFXPrefab; // Prefab untuk efek suara lompat
    

    private Rigidbody2D rb;
    public bool isGrounded;
    public bool isWalkingSoundPlaying = false; // Variabel untuk melacak status suara berjalan

    private bool isRunningSoundPlaying = false;
    public float runMultiplier = 1.5f; // Faktor pengali kecepatan lari

    void Start()
    {   

        playerItemData.dapetKaca = false; // Reset status kaca saat mulai
        playerItemData.dapetKeris = false; // Reset status keris saat mulai
        playerItemData.dapetKafan = false; // Reset status kafan saat mulai

        if (globalVolume != null && globalVolume.profile != null)
        {
            globalVolume.profile.TryGet(out vignette);
        }
        


        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Ambil komponen Animator dari GameObject ini
        spriteRenderer = GetComponent<SpriteRenderer>(); // Ambil komponen SpriteRenderer dari GameObject ini
    }

    void Update()
    {   
        if (!canMove)
        {   
            return;
        }

        if (Input.GetKeyDown(KeyCode.F) && isGrounded)
        {   
            if(playerItemData.dapetKafan)
            {   
                vignette.intensity.value = 0.85f; // Ubah intensitas vignette saat crouch
                isCrounching = true;
                rb.velocity = new Vector2(0, rb.velocity.y); // Reset kecepatan horizontal saat crouch
                animator.SetBool("isCrouch", true); // Set animator ke crouch
                lampu.SetActive(false); // Matikan lampu saat crouch
                AudioManager.Instance.PlaySFX("PlayerMovement",1);
            }else if(playerItemData.dapetKeris)
            {


            }else if(playerItemData.dapetKaca)
            {
               Flashbang.instance.ActiveFlashBang();
               playerItemData.dapetKaca = false; // Hapus kaca setelah digunakan
            }
            
        }

        if (Input.GetKeyUp(KeyCode.F) && isGrounded)
        {
            if (playerItemData.dapetKafan)
            {
                vignette.intensity.value = 0f; // Kembalikan intensitas vignette saat tidak crouch
                animator.SetBool("isCrouch", false); // Set animator ke idle
                isCrounching = false; // Keluar dari crouch
                lampu.SetActive(true); // Nyalakan lampu saat tidak crouch
            }
            else if (playerItemData.dapetKeris)
            {
                if (Input.GetAxis("Horizontal") != 0)
                {
                    string currentScene = SceneManager.GetActiveScene().name;

                    if (currentScene == "Stage 3")
                    {
                        AudioManager.Instance.PlaySFX("PlayerMovement", 3); // Play SFX untuk Stage3
                    }
                    else
                    {
                        AudioManager.Instance.PlaySFX("PlayerMovement", 0); // Play SFX default
                    }

                    isWalkingSoundPlaying = true; // Set flag ke true
                }

            }
            else if (playerItemData.dapetKaca)
            {

            }

            
        }

        // Hanya bergerak jika tidak crouch
        if (!isCrounching)
        {
            float move = Input.GetAxisRaw("Horizontal");
            float speed = moveSpeed;

            // Cek apakah tombol Shift ditekan dan player sedang grounded
            bool isRunning = Input.GetKey(KeyCode.LeftShift) && isGrounded && move != 0;

            // Set animasi run sesuai status isRunning
            animator.SetBool("isRunning", isRunning);

            if (isRunning)
            {
                speed *= runMultiplier;
            }

            rb.velocity = new Vector2(move * speed, rb.velocity.y);

            // Flip sprite berdasarkan arah pergerakan
            if (move < 0)
            {
                spriteRenderer.flipX = true; // Balik sprite ke kiri
                lampu.transform.localPosition = new Vector3(-0.53f, 0.707f, lampu.transform.localPosition.z); // Pindahkan lampu ke kiri
            }
            else if (move > 0)
            {
                spriteRenderer.flipX = false; // Balik sprite ke kanan
                lampu.transform.localPosition = new Vector3(0.53f, 0.707f, lampu.transform.localPosition.z); // Pindahkan lampu ke kanan
            }
        }
        else
        {
            // Jika crouch, pastikan animasi run juga false
            animator.SetBool("isRunning", false);
        }

        // Cek apakah menyentuh tanah
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Lompat jika di tanah
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrounching)
        {
            Debug.Log("Memutar SFX lompat");
            GameObject jumptsfx = Instantiate(jumpSFXPrefab, transform.position, Quaternion.identity);

            Destroy(jumptsfx, 3f); 


            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetBool("isJump", true); // Set animator ke lompat (ganti dari trigger ke bool)
        }

        // Setelah update posisi, cek status lompat
        if (!isGrounded)
        {
            animator.SetBool("isJump", true); // Selama di udara, isJump tetap true
        }
        else
        {
            animator.SetBool("isJump", false); // Saat menyentuh tanah, isJump jadi false
        }

        if (Input.GetAxis("Horizontal") != 0 && !isCrounching)
        {
            animator.SetBool("isWalking", true); // Set animator ke berjalan

            float move = Input.GetAxisRaw("Horizontal");

            bool isRunning = Input.GetKey(KeyCode.LeftShift) && isGrounded && move != 0;

            string currentScene = SceneManager.GetActiveScene().name;

            // Hentikan suara sebelumnya jika arah berubah
            if ((move < 0 && spriteRenderer.flipX == false) || (move > 0 && spriteRenderer.flipX == true))
            {
                if (currentScene == "Stage 3")
                {
                    AudioManager.Instance.StopSFX("PlayerMovement", 3);
                }
                else
                {
                    AudioManager.Instance.StopSFX("PlayerMovement", 0);
                }

                isRunningSoundPlaying = false;
                isWalkingSoundPlaying = false; // Reset flag untuk memutar ulang suara
            }

            // Putar suara berjalan atau berlari berdasarkan kondisi
            if (isRunning)
            {
                if (!isRunningSoundPlaying)
                { // Reset flag untuk memutar ulang suara
                    Debug.Log("Memutar SFX berlari");
                    if (currentScene == "Stage 3")
                    {
                        AudioManager.Instance.PlaySFXWithPitch("PlayerMovement", 3, 3f); // Play SFX untuk Stage3 dengan pitch lebih tinggi saat berlari
                    }
                    else
                    {
                        AudioManager.Instance.PlaySFXWithPitch("PlayerMovement", 0, 1.8f); // Play SFX default dengan pitch lebih tinggi saat berlari
                    }
                    isRunningSoundPlaying = true;
                    isWalkingSoundPlaying = false; 
                }
            }
            else if (!isWalkingSoundPlaying) // Putar suara berjalan hanya jika belum diputar
            {
                Debug.Log("Memutar SFX jalan");
                if (currentScene == "Stage 3")
                {
                    AudioManager.Instance.PlaySFXWithPitch("PlayerMovement", 3, 2.5f); // Play SFX untuk Stage3 dengan pitch normal saat berjalan
                }
                else
                {
                    AudioManager.Instance.PlaySFXWithPitch("PlayerMovement", 0, 1.2f); // Play SFX default dengan pitch normal saat berjalan
                }

                isRunningSoundPlaying = false;
                isWalkingSoundPlaying = true; // Tandai bahwa suara berjalan sedang diputar
            }
        }
        else
        {
            animator.SetBool("isWalking", false); // Set animator ke idle

            if (isWalkingSoundPlaying) // Hanya hentikan suara jika sedang diputar
            {
                string currentScene = SceneManager.GetActiveScene().name;

                if (currentScene == "Stage 3")
                {
                    AudioManager.Instance.StopSFX("PlayerMovement", 3); // Hentikan SFX untuk Stage3
                }
                else
                {
                    AudioManager.Instance.StopSFX("PlayerMovement", 0); // Hentikan SFX default
                }

                isWalkingSoundPlaying = false;
                isRunningSoundPlaying = false;
            }
        }

        // Tambahan: Matikan suara berjalan jika player tidak di tanah
        if (!isGrounded)
        {
            AudioManager.Instance.StopSFX("PlayerMovement", 0);
            AudioManager.Instance.PlaySFX("PlayerMovement", 4);
            isWalkingSoundPlaying = false;
            isRunningSoundPlaying = false;
        }

    }

}

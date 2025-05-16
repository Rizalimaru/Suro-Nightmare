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
    

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isWalkingSoundPlaying = false; // Variabel untuk melacak status suara berjalan

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
                animator.SetTrigger("Keris");
                Debug.Log("done");
                AudioManager.Instance.StopSFX("PlayerMovement", 0);
                AudioManager.Instance.PlaySFX("Stage2", 0);
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
            rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

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

        // Cek apakah menyentuh tanah
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Lompat jika di tanah
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrounching)
        {
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

        // Logika untuk suara berjalan
        if (Input.GetAxis("Horizontal") != 0 && !isCrounching)
        {
            animator.SetBool("isWalking", true); // Set animator ke running

            if (!isWalkingSoundPlaying) // Hanya mainkan suara jika belum diputar
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

                isWalkingSoundPlaying = true;
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
            }
        }

        // Tambahan: Matikan suara berjalan jika player tidak di tanah
        if (!isGrounded)
        {
            AudioManager.Instance.StopSFX("PlayerMovement", 0);
            AudioManager.Instance.PlaySFX("PlayerMovement", 4);
            isWalkingSoundPlaying = false;
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public bool isCrounching = false;
    public bool canMove = true; // Variabel untuk mengontrol gerakan
    private Animator animator; // Tambahkan variabel animator
    private SpriteRenderer spriteRenderer; // Tambahkan referensi ke SpriteRenderer
    public GameObject lampu;
    public PlayerItemData playerItemData;
    private kerisEffect Stun;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isWalkingSoundPlaying = false; // Variabel untuk melacak status suara berjalan

    void Start()
    {
        Stun = GetComponent<kerisEffect>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Ambil komponen Animator dari GameObject ini
        spriteRenderer = GetComponent<SpriteRenderer>(); // Ambil komponen SpriteRenderer dari GameObject ini
    }

    void Update()
    {   
        if (!canMove) return; // Jika tidak bisa bergerak, keluar dari fungsi

        if (Input.GetKeyDown(KeyCode.F) && isGrounded)
        {   
            if(playerItemData.dapetKafan)
            {
                isCrounching = true;
                rb.velocity = new Vector2(0, rb.velocity.y); // Reset kecepatan horizontal saat crouch
                animator.SetBool("isCrouch", true); // Set animator ke crouch
                lampu.SetActive(false); // Matikan lampu saat crouch
            }else if(playerItemData.dapetKeris)
            {
                animator.SetTrigger("Keris");
                Debug.Log("done");
                Stun.TriggerStun();
            }else if(playerItemData.dapetKaca)
            {
               Flashbang.instance.ActiveFlashBang();
               playerItemData.dapetKaca = false; // Hapus kaca setelah digunakan
            }
            
        }

        if (Input.GetKeyUp(KeyCode.F) && isGrounded)
        {   
            if(playerItemData.dapetKafan)
            {
                animator.SetBool("isCrouch", false); // Set animator ke idle
                isCrounching = false; // Keluar dari crouch
                lampu.SetActive(true); // Nyalakan lampu saat tidak crouch
            }else if(playerItemData.dapetKeris)
            {

            }else if(playerItemData.dapetKaca)
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
            animator.SetTrigger("Jump"); // Set animator ke jump
        }

        // Logika untuk suara berjalan
        if (Input.GetAxis("Horizontal") != 0 && !isCrounching)
        {
            animator.SetBool("isWalking", true); // Set animator ke running

            if (!isWalkingSoundPlaying) // Hanya mainkan suara jika belum diputar
            {
                AudioManager.Instance.PlaySFX("PlayerMovement", 0);
                isWalkingSoundPlaying = true;
            }
        }
        else
        {
            animator.SetBool("isWalking", false); // Set animator ke idle

            if (isWalkingSoundPlaying) // Hanya hentikan suara jika sedang diputar
            {
                AudioManager.Instance.StopSFX("PlayerMovement", 0);
                isWalkingSoundPlaying = false;
            }
        }
    }

}

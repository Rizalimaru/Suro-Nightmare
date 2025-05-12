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

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Ambil komponen Animator dari GameObject ini
        spriteRenderer = GetComponent<SpriteRenderer>(); // Ambil komponen SpriteRenderer dari GameObject ini
    }

    void Update()
    {   
        if (!canMove) return; // Jika tidak bisa bergerak, keluar dari fungsi

        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrounching = true;
            rb.velocity = new Vector2(0, rb.velocity.y); // Reset kecepatan horizontal saat crouch
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            isCrounching = false; // Keluar dari crouch
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
        }

        if (Input.GetAxis("Horizontal") != 0 & !isCrounching)
        {
            animator.SetBool("isWalking", true); // Set animator ke running
        }
        else
        {
            animator.SetBool("isWalking", false); // Set animator ke idle
        }
    }
}

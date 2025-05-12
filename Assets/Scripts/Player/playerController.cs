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

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        }

        // Cek apakah menyentuh tanah
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Lompat jika di tanah
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrounching)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}

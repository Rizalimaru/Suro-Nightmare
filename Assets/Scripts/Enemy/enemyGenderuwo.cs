using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyGenderuwo : MonoBehaviour
{
    public float patrolSpeed = 2f; // Kecepatan jalan musuh
    public float detectionRange = 3f; // Jarak deteksi pemain
    public float stopDuration = 2f; // Durasi berhenti saat mendekati pemain
    public Transform groundCheck; // Posisi untuk memeriksa ujung ground
    public LayerMask groundLayer; // Layer untuk ground

    private Rigidbody2D rb;
    private bool isFacingRight = true;
    private bool isStopping = false;
    private float stopTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isStopping)
        {
            // Hitung waktu berhenti
            stopTimer += Time.deltaTime;
            if (stopTimer >= stopDuration)
            {
                isStopping = false;
                stopTimer = 0f;
            }
            return; // Jangan bergerak saat berhenti
        }

        // Patroli
        Patrol();

        // Deteksi pemain
        DetectPlayer();
    }

    void Patrol()
    {
        // Gerakan horizontal
        float moveDirection = isFacingRight ? 1 : -1;
        rb.velocity = new Vector2(moveDirection * patrolSpeed, rb.velocity.y);

        // Periksa ujung ground
        if (!Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer))
        {
            Flip();
        }
    }

    void DetectPlayer()
    {
        // Cari pemain di sekitar
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectionRange, LayerMask.GetMask("Player"));
        if (player != null)
        {
            // Berhenti sejenak jika pemain terdeteksi
            isStopping = true;
            rb.velocity = Vector2.zero;
        }
    }

    void Flip()
    {
        // Balik arah musuh
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    void OnDrawGizmosSelected()
    {
        // Gambar lingkaran untuk jarak deteksi pemain di editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyGenderuwo : MonoBehaviour
{
    public float patrolSpeed = 2f; // Kecepatan jalan musuh
    public float detectionRange = 18f; // Jarak awal untuk mulai bergerak
    public float maxPatrolDistance = 30f; // Jarak maksimum patrol dari pemain
    public Transform groundCheck; // Posisi untuk memeriksa ujung ground
    public LayerMask groundLayer; // Layer untuk ground

    private Rigidbody2D rb;
    private bool isFacingRight = true;
    public Transform player;
    private bool isFollowingPlayer = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Debug.Log("Distance to Player: " + distanceToPlayer);

        // Jika bersentuhan dengan pemain dan pemain tidak crouch, follow player
        if (isFollowingPlayer)
        {
            FollowPlayer();
            return;
        }

        // Jika jarak terlalu jauh dari pemain, balik arah
        if (distanceToPlayer > maxPatrolDistance)
        {
            Flip();
        }

        // Patrol ke arah pemain
        PatrolTowardsPlayer();
    }

    void PatrolTowardsPlayer()
    {
        float moveDirection = isFacingRight ? 1 : -1;
        rb.velocity = new Vector2(moveDirection * patrolSpeed, rb.velocity.y);

        // Periksa ujung ground
        if (!Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer))
        {
            Flip();
        }
    }

    void FollowPlayer()
    {
        float moveDirection = player.position.x > transform.position.x ? 1 : -1;
        rb.velocity = new Vector2(moveDirection * patrolSpeed, rb.velocity.y);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerController playerController = collision.GetComponent<playerController>();
            if (playerController != null)
            {
                player = collision.transform;

                // Jika pemain tidak crouch, mulai follow player
                if (!playerController.isCrounching)
                {
                    isFollowingPlayer = true;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerController playerController = collision.GetComponent<playerController>();
            if (playerController != null)
            {
                // Jika pemain keluar dari trigger, berhenti follow
                isFollowingPlayer = false;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Gambar lingkaran untuk jarak deteksi pemain di editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxPatrolDistance);
    }
}

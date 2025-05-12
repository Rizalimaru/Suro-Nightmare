using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyPocong : MonoBehaviour
{
   [Header("Target")]
    public Transform player;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float chaseRange = 10f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop moving when out of range
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    // Optional: Visualize chase range in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}

using System.Collections;
using UnityEngine;

public class enemyPocong : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float chaseRange = 10f;

    private Rigidbody2D rb;
    private Collider2D col;

    private bool isStunned = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (isStunned || player == null)
        {
            rb.velocity = Vector2.zero; // Diam jika stun
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    public void Stun(float duration)
    {
        if (!isStunned)
        {
            StartCoroutine(StunRoutine(duration));
        }
    }

    private IEnumerator StunRoutine(float duration)
    {
        isStunned = true;

        if (col != null)
            col.enabled = false; // Matikan collider agar bisa dilewati

        rb.velocity = Vector2.zero; // Hentikan gerakan

        yield return new WaitForSeconds(duration);

        if (col != null)
            col.enabled = true; // Aktifkan kembali collider

        isStunned = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}

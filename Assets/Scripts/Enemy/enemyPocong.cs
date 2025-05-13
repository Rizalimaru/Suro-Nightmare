using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enemyPocong : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float chaseRange = 10f;

    private Transform player;
    private Rigidbody2D rb;
    private Collider2D col;
    private Animator anim; 

    private bool isStunned = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>(); 

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        FacePlayer();
    }

    void Update()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        if (isStunned || player == null)
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("isChasing", false);
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
            anim.SetBool("isChasing", false);
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;

        FacePlayer();
        SetChasingAnimation(true); // 🔹 Set animasi mengejar
    }

    void FacePlayer()
    {
        if (player == null) return;

        Vector3 scale = transform.localScale;

        if (player.position.x > transform.position.x)
            scale.x = Mathf.Abs(scale.x); // Menghadap kanan
        else
            scale.x = -Mathf.Abs(scale.x); // Menghadap kiri

        transform.localScale = scale;
    }

    void SetChasingAnimation(bool isChasing)
    {
        if (anim != null)
            anim.SetBool("isChasing", isChasing); // 🔹 Ubah parameter animator
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
        SetChasingAnimation(false); // 🔹 Set animasi idle saat stun

        if (col != null)
            col.enabled = false;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForSeconds(duration);

        if (col != null)
            col.enabled = true;

        rb.gravityScale = originalGravity;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        isStunned = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}

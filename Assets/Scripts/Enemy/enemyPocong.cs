using System.Collections;
using UnityEngine;

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

    public UIPause uipause; // Referensi ke UI Pause

    private Stage2GameOver stage2GameOverScript;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        // Ambil reference ke script Stage2GameOver di scene
        stage2GameOverScript = FindObjectOfType<Stage2GameOver>();

        FacePlayer();
    }

    void Update()
    {
        // Cek game over dari script Stage2GameOver
        if (stage2GameOverScript != null && stage2GameOverScript.isGameOver)
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("isChasing", false);
            return;
        }

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
        SetChasingAnimation(true);
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
            anim.SetBool("isChasing", isChasing);
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
        SetChasingAnimation(false);

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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}

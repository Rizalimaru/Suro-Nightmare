using UnityEngine;
using System.Collections;

public class kerisEffect : MonoBehaviour
{
    public KeyCode stunKey = KeyCode.K;
    public float stunRadius = 5f;
    public float stunDuration = 3f;

    private Animator anim;
    void Start()
    {
        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(stunKey))
        {
            anim.SetTrigger("Keris");
            TriggerStun();
            AudioManager.Instance.PlaySFX("Stage2", 0);
        }
    }

    public void TriggerStun()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, stunRadius);

        foreach (Collider2D col in hitObjects)
        {
            if (col.CompareTag("Pocong"))
            {
                // Memanggil metode Stun dari enemyPocong untuk memberikan efek stun
                enemyPocong ai = col.GetComponent<enemyPocong>();
                if (ai != null)
                {
                    ai.Stun(stunDuration);  // Memberikan efek stun pada pocong
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, stunRadius);
    }
}

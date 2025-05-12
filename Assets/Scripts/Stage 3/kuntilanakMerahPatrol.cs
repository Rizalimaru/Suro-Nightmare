using UnityEngine;

public class KuntilanakMerahPatrol : MonoBehaviour
{
    public Transform[] waypoints; // urutan: lantai 3 kiri > kanan > tangga turun > lantai 2 kanan > kiri > tangga naik
    public float speed = 2f;
    private int currentWaypointIndex = 0;
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    public float rotateAngleSlopeDown = -35f;
    public float rotateAngleSlopeUp = 35f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypointIndex];
        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * speed;

        // Flip sprite based on direction (x axis)
        if (direction.x > 0)
            sr.flipX = false;
        else if (direction.x < 0)
            sr.flipX = true;

        // Rotation adjustment for slope
        if (target.name.Contains("SlopeDown"))
        {
            transform.rotation = Quaternion.Euler(0, 0, rotateAngleSlopeDown);
        }
        else if (target.name.Contains("SlopeUp"))
        {
            transform.rotation = Quaternion.Euler(0, 0, rotateAngleSlopeUp);
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }

        // Check if arrived at waypoint
        if (Vector2.Distance(transform.position, target.position) < 0.2f)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                // Patrol balik ke awal
                System.Array.Reverse(waypoints);
                currentWaypointIndex = 0;
            }
        }
    }
}

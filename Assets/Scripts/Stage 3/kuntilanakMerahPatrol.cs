using UnityEngine;

public class KuntilanakMerahPatrol : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 2f;
    private int currentWaypointIndex = 0;

    public float rotateAngleSlopeDown = -35f;
    public float rotateAngleSlopeUp = 35f;

    void Update()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypointIndex];
        Vector2 direction = (target.position - transform.position).normalized;

        // Gerak pakai transform.position langsung
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Flip skala berdasarkan arah horizontal
        if (direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1);  // Menghadap kanan
        else if (direction.x < 0)
            transform.localScale = new Vector3(-1, 1, 1); // Menghadap kiri

        // Rotasi saat melewati tangga
        if (target.name.Contains("SlopeDown"))
        {
            transform.rotation = Quaternion.Euler(0, 0, rotateAngleSlopeDown);
        }
        else
        {
            transform.rotation = Quaternion.identity; // Pastikan rotasi kembali normal jika tidak melewati slope
        }

        // Cek jarak sampai waypoint
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            currentWaypointIndex++;

            // Jika sudah sampai di waypoint terakhir, kembali ke waypoint pertama
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }



        
    }
}

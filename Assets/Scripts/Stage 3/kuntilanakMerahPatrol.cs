using Unity.VisualScripting;
using UnityEngine;

public class KuntilanakMerahPatrol : MonoBehaviour
{

    public static KuntilanakMerahPatrol instance;
    public Transform[] waypoints;
    public float speed = 2f;


    public Transform spawnPoint;        // Tempat player akan dipindahkan saat kena kuntilanak
    public string playerTag = "Player"; // Pastikan player pakai tag ini
    private int currentWaypointIndex = 0;

    public float rotateAngleSlopeDown = -35f;
    public float rotateAngleSlopeUp = 35f;

    private GameObject player;
    private SembunyiLemari sembunyi;
    public BoxCollider2D myCollider;

    public SembunyiLemari lemari;

    void Start()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            sembunyi = player.GetComponent<SembunyiLemari>();

        myCollider =GetComponent<BoxCollider2D>();

    }

    void Update()
    {


        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypointIndex];
        Vector2 direction = (target.position - transform.position).normalized;

        // Gerak pakai transform.position langsung
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Flip skala berdasarkan arah horizontal
        if (direction.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);  // Menghadap kanan
        else if (direction.x < 0)
            transform.localScale = new Vector3(1, 1, 1); // Menghadap kiri

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


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            Debug.Log("KENA LU");
            GameObject player = other.gameObject;

            // Cek apakah player sedang bersembunyi
            SembunyiLemari sembunyi = player.GetComponent<SembunyiLemari>();
            if (sembunyi != null && sembunyi.IsHiding)
            {
                Debug.Log("Tapi player sedang sembunyi, tidak kena");
                return; // Tidak kena kalau sedang sembunyi
            }

            // Reset posisi player
            if (spawnPoint != null)
            {
                player.transform.position = spawnPoint.position;
            }
        }
    }

    public void SetCollisionKunti( string benar){

    }



    private bool IsHiding(SembunyiLemari sembunyiScript)
    {
        // Mengakses variabel private isHiding menggunakan refleksi
        return (bool)sembunyiScript.GetType().GetField("isHiding", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(sembunyiScript);
    }
}

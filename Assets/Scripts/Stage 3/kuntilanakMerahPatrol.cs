using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KuntilanakMerahPatrol : MonoBehaviour
{
    //GameOve Logic
    public GameObject gameOverUI; // Referensi ke UI Game Over
    public Animator gameOverAnimator; // Referensi ke Animator untuk Game Over
    public playerController playerController; // Referensi ke skrip playerController
    public GameObject tombolUlang; // Referensi ke tombol Ulang

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

    public teleportStage3_1 teleportStage3_1;

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
                myCollider.enabled = false; // Disable collider kuntilanak
                //animasi transisi Hehe
                StartCoroutine(GameOver());
                // teleportStage3_1.teleportTransition();
                // StartCoroutine(WaitForSeconds(2f));
            }
        }
    }

    public IEnumerator GameOver()
    {   
        tombolUlang.SetActive(false);
        gameOverUI.SetActive(true);
        gameOverAnimator.SetTrigger("gameOver");
        yield return new WaitForSeconds(2f); // Tunggu 2 detik sebelum menampilkan Game Over
        tombolUlang.SetActive(true);
        AudioListener.volume = 0;
        //Time.timeScale = 0; // Hentikan permainan
        //hentikan semua suara
    }

    IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        player.transform.position = spawnPoint.position;
        myCollider.enabled = true;
    } 



    public void SetCollisionKunti( string benar){

    }



    private bool IsHiding(SembunyiLemari sembunyiScript)
    {
        // Mengakses variabel private isHiding menggunakan refleksi
        return (bool)sembunyiScript.GetType().GetField("isHiding", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(sembunyiScript);
    }
}

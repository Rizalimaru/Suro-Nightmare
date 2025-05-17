using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GlassFragment : MonoBehaviour
{
    private bool isPlayerNear = false;
    private GameObject player;
    private GlassCollector manager;
    public GameObject interactText; // assign dari inspector atau pakai GetComponentInChildren()

    public UnityEngine.UI.Image glassImage;

    public string glassTextValue ; // Teks yang akan ditampilkan saat kaca ditemukan
    public Sprite cermin;


    public GameObject glassSFXPrefab; // Tambahkan variabel untuk prefab SFX

    // teks yang bisa diubah dari inspector

    public GameObject uiGlassFound;

     // Referensi ke UI Pause

    void Start()
    {
        manager = FindObjectOfType<GlassCollector>();
        interactText.SetActive(false); // sembunyikan di awal
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            manager.CollectFragment(gameObject);
            isPlayerNear = false;
            interactText.SetActive(false);

            if(glassSFXPrefab != null)
            {
                Instantiate(glassSFXPrefab, transform.position, Quaternion.identity);

            }
            }


            //StartCoroutine(HideGlassFoundUI()); // Mulai coroutine untuk menyembunyikan UI setelah beberapa detik

        }



    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            interactText.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            interactText.SetActive(false);
        }
    }


}

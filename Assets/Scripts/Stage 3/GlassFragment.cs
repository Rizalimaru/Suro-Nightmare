using UnityEngine;

public class GlassFragment : MonoBehaviour
{
    private bool isPlayerNear = false;
    private GameObject player;
    private GlassCollector manager;
    public GameObject interactText; // assign dari inspector atau pakai GetComponentInChildren()

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
        }
    }

    void OnTriggerEnter2D(Collider2D other)
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

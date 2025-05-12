using UnityEngine;

public class teleportStage3_1 : MonoBehaviour
{
    public Transform[] targetTeleport;
    public GameObject interactionText;

    private bool playerInRange = false;
    private Transform playerTransform;

    void Start()
    {
        interactionText.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Teleport player ke targetTeleport[0]
            playerTransform.position = targetTeleport[0].position;
            interactionText.SetActive(false);
            playerInRange = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactionText.SetActive(true);
            playerInRange = true;
            playerTransform = collision.transform;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactionText.SetActive(false);
            playerInRange = false;
        }
    }
}

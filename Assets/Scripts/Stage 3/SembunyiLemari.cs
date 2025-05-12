using UnityEngine;
using TMPro;

public class SembunyiLemari : MonoBehaviour
{
    private bool canHide = false;
    private bool isHiding = false;
    private GameObject player;

    public TextMeshProUGUI hideTextUI; // drag UI Text ke sini via inspector

    void Start()
    {
        if (hideTextUI != null)
            hideTextUI.gameObject.SetActive(false);
    }

    void Update()
    {
        if (canHide && Input.GetKeyDown(KeyCode.E))
        {
            isHiding = !isHiding;

            var sr = player.GetComponent<SpriteRenderer>();
            var rb = player.GetComponent<Rigidbody2D>();
            var controller = player.GetComponent<playerController>();

            sr.enabled = !isHiding;
            controller.enabled = !isHiding;

            if (isHiding)
                rb.velocity = Vector2.zero;

            UpdateHideText(); // Update isi teks
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canHide = true;
            player = other.gameObject;

            if (hideTextUI != null)
            {
                hideTextUI.gameObject.SetActive(true);
                UpdateHideText();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canHide = false;

            if (hideTextUI != null)
                hideTextUI.gameObject.SetActive(false);
        }
    }

    void UpdateHideText()
    {
        if (hideTextUI == null) return;

        if (isHiding)
            hideTextUI.text = "Tekan E untuk keluar";
        else
            hideTextUI.text = "Tekan E untuk bersembunyi";
    }
}

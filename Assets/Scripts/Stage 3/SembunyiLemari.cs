using UnityEngine;
using TMPro;

public class SembunyiLemari : MonoBehaviour
{
    private bool canHide = false;
    public bool isHiding = false;
    private GameObject player;

    public bool IsHiding => isHiding;

    public TextMeshProUGUI hideTextUI;

    public BoxCollider2D [] kuntilanakCollider;

    public GameObject lampu2d;

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
            lampu2d.SetActive(!isHiding);
            foreach (BoxCollider2D collider in kuntilanakCollider)
            {
                if (collider != null) collider.enabled = !isHiding;
            }

            if (isHiding)
                rb.velocity = Vector2.zero;

            UpdateHideText();
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

using UnityEngine;
using System.Collections;

public class teleportStage3_1 : MonoBehaviour
{
    public Transform[] targetTeleport;
    public GameObject interactionText;

    public CanvasGroup transitionCanvas; // CanvasGroup untuk transisi

    private bool playerInRange = false;
    private Transform playerTransform;

    public DetectTeleportStage3 detectTeleportStage3;

    private bool isTeleporting = false;

    public UIPause uipause; // Referensi ke UI Pause

    void Start()
    {
        interactionText.SetActive(false);
        if (transitionCanvas != null)
        {
            transitionCanvas.alpha = 0; // Pastikan canvas transparan di awal
            transitionCanvas.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && detectTeleportStage3.isTeleporting == false )
        
        {
           detectTeleportStage3.isTeleporting = true;
            StartCoroutine(TeleportWithTransition());
        }
    }

    private IEnumerator TeleportWithTransition()
    {
        uipause.enabled = false;
        // Aktifkan canvas dan mulai transisi menghitam
        if (transitionCanvas != null)
        {
            transitionCanvas.gameObject.SetActive(true);
            yield return StartCoroutine(FadeCanvas(0, 1, 1f)); // Fade in (menghitam)
        }

        // Teleport player ke targetTeleport[0]
        playerTransform.position = targetTeleport[0].position;

        // Tunggu sebentar sebelum memulai transisi memudar
        yield return new WaitForSeconds(1f);

        // Mulai transisi memudar
        if (transitionCanvas != null)
        {
            yield return StartCoroutine(FadeCanvas(1, 0, 1f)); // Fade out (memudar)
            transitionCanvas.gameObject.SetActive(false);
        }


        interactionText.SetActive(false);
        playerInRange = false;

        yield return new WaitForSeconds(0.3f);
        detectTeleportStage3.isTeleporting = false;
        uipause.enabled = true;
    }

    public void teleportTransition()
    {
 
        StartCoroutine(PlayerWithTransition()); // Fade in (menghitam)
        
    }


     private IEnumerator PlayerWithTransition()
    {
        // Aktifkan canvas dan mulai transisi menghitam
        if (transitionCanvas != null)
        {
            transitionCanvas.gameObject.SetActive(true);
            yield return StartCoroutine(FadeCanvas(0, 1, 1f)); // Fade in (menghitam)
        }


        // Tunggu sebentar sebelum memulai transisi memudar
        yield return new WaitForSeconds(1.2f);

        // Mulai transisi memudar
        if (transitionCanvas != null)
        {
            yield return StartCoroutine(FadeCanvas(1, 0, 1f)); // Fade out (memudar)
            transitionCanvas.gameObject.SetActive(false);
        }


    }

    private IEnumerator FadeCanvas(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            transitionCanvas.alpha = alpha;
            yield return null;
        }
        transitionCanvas.alpha = endAlpha;
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
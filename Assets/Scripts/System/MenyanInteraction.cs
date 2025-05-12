using UnityEngine;
using UnityEngine.UI;

public class MenyanInteraction : MonoBehaviour
{
    [Header("Player Detection")]
    public Transform player;
    public float interactRange = 2f;

    [Header("Interaction Settings")]
    public KeyCode interactKey = KeyCode.F;
    public float progressIncreaseSpeed = 1f;   // per second saat F ditekan
    public float progressDecreaseSpeed = 0.5f; // per second saat F dilepas
    public float requiredProgress = 3f;        // nilai yang harus dicapai untuk mematikan menyan

    private float currentProgress = 0f;
    private bool isInRange = false;
    private bool isMenyanOn = true;

    [Header("UI")]
    public GameObject progressBarUI;
    public Image progressFill;

    [Header("Menyan Visuals")]
    public GameObject smokeEffect;

    void Start()
    {
        progressBarUI.SetActive(false);
        progressFill.fillAmount = 0f;
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        isInRange = distance < interactRange;

        if (isMenyanOn && isInRange)
        {
            progressBarUI.SetActive(true);

            if (Input.GetKey(interactKey))
            {
                currentProgress += progressIncreaseSpeed * Time.deltaTime;
            }
            else
            {
                currentProgress -= progressDecreaseSpeed * Time.deltaTime;
            }

            // Clamp value agar tidak lebih dari batas
            currentProgress = Mathf.Clamp(currentProgress, 0f, requiredProgress);
            progressFill.fillAmount = currentProgress / requiredProgress;

            if (currentProgress >= requiredProgress)
            {
                TurnOffMenyan();
            }
        }
        else
        {
            // Player keluar dari area
            progressBarUI.SetActive(false);
        }
    }

    void TurnOffMenyan()
    {
        isMenyanOn = false;
        progressBarUI.SetActive(false);
        progressFill.fillAmount = 0f;
        if (smokeEffect) smokeEffect.SetActive(false);
        Debug.Log("Menyan berhasil dimatikan!");
    }

    // Visual bantu di editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}

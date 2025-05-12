using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class menyanInteraction : MonoBehaviour
{
    [Header("Player Detection")]
    public Transform player;
    public float interactRange = 2f;

    [Header("Interaction Settings")]
    public KeyCode interactKey;
    public float progressIncreaseSpeed;   // per detik saat F ditekan
    public float progressDecreaseSpeed; // per detik saat F dilepas
    public float requiredProgress;        // nilai total untuk mematikan menyan

    private float currentProgress = 0f;
    private bool isInRange = false;
    private bool isMenyanOn = true;

    [Header("UI")]
    public GameObject progressBarUI;
    public Slider progressSlider;
    public GameObject InteractionHint;

    [Header("Add On")]
    public GameObject smokeEffect;
    private progressObjektif Progress;

    void Start()
    {
        Progress = FindObjectOfType<progressObjektif>();
        progressSlider.maxValue = requiredProgress;
        progressSlider.value = 0f;
        progressBarUI.SetActive(false);
        InteractionHint.SetActive(false);
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        isInRange = distance < interactRange;

        if (isMenyanOn && isInRange)
        {
            progressBarUI.SetActive(true);
            InteractionHint.SetActive(true);

            if (Input.GetKey(interactKey))
            {
                currentProgress += progressIncreaseSpeed * Time.deltaTime;
            }
            else
            {
                currentProgress -= progressDecreaseSpeed * Time.deltaTime;
            }

            // Clamp nilai
            currentProgress = Mathf.Clamp(currentProgress, 0f, requiredProgress);
            progressSlider.value = currentProgress;

            if (currentProgress >= requiredProgress)
            {
                TurnOffMenyan();
            }
        }
        else
        {
            progressBarUI.SetActive(false);
            InteractionHint.SetActive(false);
        }
    }

    void TurnOffMenyan()
    {
        isMenyanOn = false;
        progressBarUI.SetActive(false);
        InteractionHint.SetActive(false);
        progressSlider.value = 0f;
        if (smokeEffect) smokeEffect.SetActive(false);
        Progress.AddProgress();
        Debug.Log("Menyan berhasil dimatikan!");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}

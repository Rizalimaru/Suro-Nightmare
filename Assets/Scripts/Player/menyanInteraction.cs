using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class menyanInteraction : MonoBehaviour
{
    [Header("Player Detection")]
    public Transform player;
    public float interactRange = 2f;

    [Header("Interaction Settings")]
    public KeyCode interactKey = KeyCode.F;
    public float progressIncreaseSpeed = 1f;   // per detik saat F ditekan
    public float progressDecreaseSpeed = 1f;   // per detik saat F dilepas
    public float requiredProgress = 3f;        // nilai total untuk mematikan menyan

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
    private spawnerPocong Pocong;

    void Start()
    {
        // Ambil referensi script progress objektif
        Progress = FindObjectOfType<progressObjektif>();

        // Ambil referensi script spawnerPocong yang satu GameObject dengan menyan
        Pocong = GetComponent<spawnerPocong>();

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

        if (smokeEffect != null)
            smokeEffect.SetActive(false);

        // Tambah progress objektif
        if (Progress != null)
            Progress.AddProgress();

        // Hancurkan Pocong yang di-spawn oleh spawner ini
        if (Pocong != null && Pocong.spawnedPocong != null)
            Destroy(Pocong.spawnedPocong);

        Debug.Log("Menyan berhasil dimatikan dan Pocong hilang!");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}

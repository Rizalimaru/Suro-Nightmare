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
    public float progressIncreaseSpeed = 1f;
    public float progressDecreaseSpeed = 1f;
    public float requiredProgress = 3f;

    private float currentProgress = 0f;
    private bool isInRange = false;
    private bool isMenyanOn = true;

    [Header("UI")]
    public GameObject progressBarUI;
    public Slider progressSlider;
    public GameObject InteractionHint;

    [Header("VFX (Particle Systems)")]
    public ParticleSystem menyanOnVFX;     // efek saat menyala
    public ParticleSystem menyanOffVFX;    // efek saat dimatikan

    private progressObjektif Progress;
    private spawnerPocong Pocong;
    private Animator anim;

    void Start()
    {
        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        Progress = FindObjectOfType<progressObjektif>();
        Pocong = GetComponent<spawnerPocong>();

        progressSlider.maxValue = requiredProgress;
        progressSlider.value = 0f;
        progressBarUI.SetActive(false);
        InteractionHint.SetActive(false);

        if (menyanOnVFX != null) menyanOnVFX.Play();
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
                anim.SetBool("isInteract", true);
                currentProgress += progressIncreaseSpeed * Time.deltaTime;

                // Set isInteracting ke true saat pemain mulai berinteraksi
                player.GetComponent<playerController>().isInteracting = true;
            }
            else
            {
                anim.SetBool("isInteract", false);
                currentProgress -= progressDecreaseSpeed * Time.deltaTime;

                // Set isInteracting ke false saat pemain berhenti berinteraksi
                player.GetComponent<playerController>().isInteracting = false;
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

            // Pastikan isInteracting diatur ke false jika pemain keluar dari jangkauan
            player.GetComponent<playerController>().isInteracting = false;
        }
    }

    void TurnOffMenyan()
    {
        isMenyanOn = false;
        progressBarUI.SetActive(false);
        InteractionHint.SetActive(false);
        progressSlider.value = 0f;
        anim.SetBool("isInteract", false);
        Destroy(gameObject);

        if (!isMenyanOn)
            menyanOnVFX.Stop();
            menyanOffVFX.Play();

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

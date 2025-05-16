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
    private playerController playerScript;

    void Start()
    {
        // Coba auto-cari player jika belum diset manual
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
                player = playerObject.transform;
        }

        // Ambil playerScript dari player
        if (player != null)
        {
            playerScript = player.GetComponent<playerController>();
            anim = player.GetComponent<Animator>();
        }
        else
        {
            Debug.LogWarning("Player tidak ditemukan! Pastikan GameObject bertag 'Player' ada di scene.");
        }

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
        if (player == null || playerScript == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        isInRange = distance < interactRange;

        if (isMenyanOn && isInRange)
        {
            progressBarUI.SetActive(true);
            InteractionHint.SetActive(true);

            if (Input.GetKey(interactKey))
            {
                anim.SetBool("isInteract", true);
                // playerScript.canMove = false;  // Nonaktifkan gerakan
                playerScript.isInteracting = true;

                currentProgress += progressIncreaseSpeed * Time.deltaTime;
            }
            else
            {
                anim.SetBool("isInteract", false);
                // playerScript.canMove = true;   // Aktifkan kembali gerakan
                playerScript.isInteracting = false;

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

            if (playerScript != null)
                playerScript.isInteracting = false;
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

        if (menyanOnVFX != null) menyanOnVFX.Stop();
        if (menyanOffVFX != null) menyanOffVFX.Play();

        if (Progress != null)
            Progress.AddProgress();

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

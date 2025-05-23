using UnityEngine;
using TMPro;
using System.Collections;

public class GlassCollector : MonoBehaviour
{
    public int totalFragments = 3;
    private int collected = 0;

    public GameObject fullMirror;

    public TextMeshProUGUI interactText;
    public TextMeshProUGUI progressText;

    public GameObject selesaiAmbilSemuaCermin;

    public GameObject uiObjective;

    public GameObject player;

    public GameObject mainCamera;

    public PlayerItemData playerItemData;

    // untuk mendeteksi cermin mana yang sudah diambil


    public TextMeshProUGUI glassText; // Teks yang akan ditampilkan saat kaca ditemukan

    public UnityEngine.UI.Image glassImage;

    public Sprite cermin2;

    public GameObject musuhTerlihat;

   
    public string glassTextValueKaca ; // Teks yang akan ditampilkan saat kaca ditemukan

    public GameObject uiGlass;
    public UIPause uipause;

    void Start()
    {
        if (interactText != null)
            interactText.gameObject.SetActive(false);

        UpdateProgressText();

        AudioManager.Instance.StopBackgroundMusicWithTransition("Stage2", 0);



        AudioManager.Instance.PlayBackgroundMusicWithTransition2("Stage3",0, 1f,0.5f);
        AudioManager.Instance.PlayBackgroundMusicWithTransition2("Stage3",1, 1f,0.5f);
    }

    public void CollectFragment(GameObject fragment)
    {

        var playerMovement = player.GetComponent<playerController>();
        var playerAnim = player.GetComponent<Animator>();

        AudioManager.Instance.StopSFX("PlayerMovement", 3);

        // Stop semua animasi player
        playerAnim.SetBool("isWalking", false);
        playerAnim.SetBool("isRunning", false);
    
        
        playerMovement.enabled = false; // Aktifkan kontrol player saat mengambil kaca
        collected++;
        var spriteRenderer = fragment.GetComponent<GlassFragment>().cermin;

        var glassTextValue = fragment.GetComponent<GlassFragment>().glassTextValue;

        glassTextValueKaca = glassTextValue; // Simpan nilai teks kaca yang ditemukan
        if (glassText != null)
        {
            glassText.text = glassTextValueKaca; // Tampilkan teks yang sesuai
        }

        cermin2  = spriteRenderer; // Simpan sprite kaca yang ditemukan
        if (glassImage != null)
        {
            glassImage.sprite = cermin2; // Tampilkan sprite kaca yang sesuai
        }


        
        uiGlass.SetActive(true); // Tampilkan UI saat kaca ditemukan
        StartCoroutine(DestroyKaca(fragment)); // Mulai coroutine untuk menyembunyikan UI setelah beberapa detik

        UpdateProgressText();

        StartCoroutine(HideGlassFoundUI()); // Sembunyikan UI setelah 2 detik

        if (collected >= totalFragments )
        {
            uipause.enabled = false;
            uiGlass.SetActive(false);

            musuhTerlihat.SetActive(false);
            //Flashbang.instance.ActiveFlashBang();

            selesaiAmbilSemuaCermin.SetActive(true);
            AudioManager.Instance.PlaySFX("Stage3", 0);

            uiObjective.SetActive(false);
            StartCoroutine(WaitAndDisable());
        }

        ShowInteractText(false);
    }

    private IEnumerator DestroyKaca(GameObject fragment)
    {
        yield return new WaitForSeconds(1f);
        Destroy(fragment);
    }

    public IEnumerator HideGlassFoundUI()
    {
        uipause.enabled = false;
        yield return new WaitForSeconds(2f);
        if (uiGlass != null)
        {
            uiGlass.SetActive(false);
        }
        var playerMovement = player.GetComponent<playerController>();

        yield return new WaitUntil(() => Input.GetAxis("Horizontal") != 0);
        AudioManager.Instance.PlaySFX("PlayerMovement", 3); // Play SFX default
        playerMovement.enabled = true;
        uipause.enabled = true; // Aktifkan kontrol player saat mengambil kaca
    }

    public void HideGlassUI()
    {
        
    }

    public void ShowInteractText(bool show)
    {
        if (interactText != null)
            interactText.gameObject.SetActive(show);
    }

    void UpdateProgressText()
    {
        if (progressText != null)
            progressText.text = $"Temukan Pecahan Kaca ({collected}/{totalFragments})";
    }

    private IEnumerator WaitAndDisable()
    {
        yield return new WaitForSeconds(5f);

        AudioManager.Instance.PlaySFX("Stage3", 1);
        yield return new WaitForSeconds(7f);

        playerItemData.dapetKaca = true;
        mainCamera.SetActive(true);
        selesaiAmbilSemuaCermin.SetActive(false);

    }
}

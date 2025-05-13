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

    public GameObject mainCamera;

    void Start()
    {
        if (interactText != null)
            interactText.gameObject.SetActive(false);

        UpdateProgressText();

        AudioManager.Instance.PlayBackgroundMusicWithTransition2("Stage3",0, 1f,0.7f);
        AudioManager.Instance.PlayBackgroundMusicWithTransition2("Stage3",1, 1f,0.7f);
    }

    public void CollectFragment(GameObject fragment)
    {
        collected++;
        Destroy(fragment);
        UpdateProgressText();

        if (collected >= totalFragments && fullMirror != null)
        {
            //Flashbang.instance.ActiveFlashBang();

            selesaiAmbilSemuaCermin.SetActive(true);
            AudioManager.Instance.PlaySFX("Stage3", 0);

            uiObjective.SetActive(false);
            StartCoroutine(WaitAndDisable());
        }

        ShowInteractText(false);
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
        yield return new WaitForSeconds(12f);
        mainCamera.SetActive(true);
        selesaiAmbilSemuaCermin.SetActive(false);

    }
}

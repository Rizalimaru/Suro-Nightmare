using UnityEngine;
using TMPro;

public class GlassCollector : MonoBehaviour
{
    public int totalFragments = 3;
    private int collected = 0;

    public GameObject fullMirror;

    public TextMeshProUGUI interactText;
    public TextMeshProUGUI progressText;

    void Start()
    {
        if (interactText != null)
            interactText.gameObject.SetActive(false);

        UpdateProgressText();
    }

    public void CollectFragment(GameObject fragment)
    {
        collected++;
        Destroy(fragment);
        UpdateProgressText();

        if (collected >= totalFragments && fullMirror != null)
        {
            fullMirror.SetActive(true);
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
}

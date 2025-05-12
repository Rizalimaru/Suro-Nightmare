using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Flashbang : MonoBehaviour
{
    public static Flashbang instance; // Singleton instance
    public Light2D flashLight;           // Drag Point Light 2D ke Inspector
    public float maxIntensity = 5f;      // Seberapa terang efeknya
    public float flashDuration = 1f;     // Durasi total flash
    private float timer = 0f;
    private bool isFlashing = false;

    public GameObject kuntilanak; // Drag kuntilanak ke Inspector

    public GameObject whiteScreen; // Drag white screen ke Inspector

    void Start()
    {
        instance = this;
        flashLight.intensity = 0f;
    }

    public void TriggerFlash()
    {
        timer = 0f;
        isFlashing = true;
    }

    void Update()
    {
        if (isFlashing)
        {
            timer += Time.deltaTime;
            float halfDuration = flashDuration / 2f;

            if (timer <= halfDuration)
            {
                // Fase naik terang
                flashLight.intensity = Mathf.Lerp(0f, maxIntensity, timer / halfDuration);
            }
            else if (timer <= flashDuration)
            {
                // Fase turun redup
                flashLight.intensity = Mathf.Lerp(maxIntensity, 0f, (timer - halfDuration) / halfDuration);
            }
            else
            {
                // Selesai
                flashLight.intensity = 0f;
                isFlashing = false;
            }
        }

        // trial dengan flashbang
        if (Input.GetKeyDown(KeyCode.F))
        {
            ActiveFlashBang();



        }
    }

    public void ActiveFlashBang(){
            // Mengaktifkan kuntilanak
        kuntilanak.SetActive(false);
        TriggerFlash();
        StartCoroutine(RemoveWhiteScreen());
    }

    private System.Collections.IEnumerator RemoveWhiteScreen()
    {
        
        yield return new WaitForSeconds(2f);
        whiteScreen.SetActive(true);
        AudioManager.Instance.StopBackgroundMusicWithTransition("Stage3",1f);
    }
}

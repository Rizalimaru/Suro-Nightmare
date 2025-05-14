using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEditor.SearchService;

public class Flashbang : MonoBehaviour
{
    public Transform player; // Drag Player ke Inspector
    public static Flashbang instance; // Singleton instance
    public Light2D flashLight;           // Drag Point Light 2D ke Inspector
    public float maxIntensity = 5f;      // Seberapa terang efeknya
    public float flashDuration = 1f;     // Durasi total flash
    private float timer = 0f;
    private bool isFlashing = false;

    public GameObject flashSFXPrefab; // Tambahkan variabel untuk prefab SFX

    public GameObject kuntilanak; // Drag kuntilanak ke Inspector

    public GameObject whiteScreen; // Drag white screen ke Inspector

    public Volume globalVolume; // Drag Global Volume di Inspector
    private Vignette vignette; // Referensi ke efek vignette

    public SceneController sceneController; // Drag SceneController ke Inspector

    void Start()
    {
        instance = this;
        flashLight.intensity = 0f;
    }

    public void TriggerFlash()
    {
        // Memindahkan flashlight ke posisi player
        flashLight.transform.position = player.position;
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
        // if (Input.GetKeyDown(KeyCode.F))
        // {
        //     ActiveFlashBang();



        // }
    }

    public void ActiveFlashBang(){
            // Mengaktifkan kuntilanak
        Instantiate(flashSFXPrefab, transform.position, Quaternion.identity);
        kuntilanak.SetActive(false);
        //disable vignette
        if (globalVolume != null && globalVolume.profile != null)
        {
            globalVolume.profile.TryGet(out vignette);
            if (vignette != null)
            {
                vignette.intensity.value = 0f; // Set ke 0 untuk menghilangkan efek
            }
        }
        TriggerFlash();
        StartCoroutine(RemoveWhiteScreen());

        
    }

    private System.Collections.IEnumerator RemoveWhiteScreen()
    {
        AudioManager.Instance.StopBackgroundMusicWithTransition("Stage3",2f);
        
        yield return new WaitForSeconds(3f);
        whiteScreen.SetActive(true);
        sceneController.LoadScene("EndingBro");
        
    }
}

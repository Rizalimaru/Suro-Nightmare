using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagementMenu : MonoBehaviour
{
    public GameObject[] panels; // isinya tampilan menu, options dan kredit
    private bool isButtonPressed = false; // Flag untuk mencegah spamming tombol

    public SceneController sceneController; // Referensi ke SceneController

    private bool isStageActive = false; // Flag untuk mengecek apakah stage aktif

    void Start()
    {
        panels[0].SetActive(true); // Menu utama aktif
        panels[1].SetActive(false); // Options tidak aktif
        panels[2].SetActive(false); // Kredit tidak aktif
        panels[3].SetActive(false); // Pilih are

        AudioManager.Instance.PlayBackgroundMusicWithTransition2("Mainmenu", 0,2f, 0.7f); // Memutar musik latar menu utama
    }

    void Update()
    {
        // Kalau panel indeks 1 atau 2 aktif, jika escape ditekan, kembali ke menu utama
        if (panels[1].activeSelf || panels[2].activeSelf || panels[3].activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                BackToMenu();
            }
        }
    }

    public void PlayGame()
    {
        if (isButtonPressed) return; // Cegah spamming tombol
        isButtonPressed = true;

        // Load scene berikutnya
        AudioManager.Instance.PlaySFX("Mainmenu", 0);
        AudioManager.Instance.StopBackgroundMusicWithTransition("Mainmenu", 1f);
        sceneController.LoadScene("Intro Story");

        StartCoroutine(ResetButtonFlag());
    }

    public void OpenOptions()
    {
        if (isButtonPressed) return; // Cegah spamming tombol
        isButtonPressed = true;

        StartCoroutine(PlaySoundAndOpenPanel(1));
        StartCoroutine(ResetButtonFlag());
    }

    public void OpenScene(string namescene){
        if (isStageActive) return; // Cegah spamming tombol
        isStageActive = true;
        // Load scene berikutnya
        AudioManager.Instance.PlaySFX("Mainmenu", 0);
        AudioManager.Instance.StopBackgroundMusicWithTransition("Mainmenu", 1f);
        sceneController.LoadScene(namescene);

        StartCoroutine(ResetStage());
    }

    private IEnumerator ResetStage()
    {
        yield return new WaitForSeconds(0.5f); // Sesuaikan durasi ini jika diperlukan
        isStageActive = true; // Reset flag setelah durasi tertentu
    }

  

    public void OpenKredit()
    {
        if (isButtonPressed) return; // Cegah spamming tombol
        isButtonPressed = true;

        StartCoroutine(PlaySoundAndOpenPanel(2));
        StartCoroutine(ResetButtonFlag());
    }

    public void OpenPilihArea()
    {
        if (isButtonPressed) return; // Cegah spamming tombol
        isButtonPressed = true;

        StartCoroutine(PlaySoundAndOpenPanel(3));
        StartCoroutine(ResetButtonFlag());
    }

    public void BackToMenu()
    {
        AudioManager.Instance.PlaySFX("Mainmenu", 0);
        panels[1].SetActive(false);
        panels[2].SetActive(false);
        panels[3].SetActive(false);
    }

    public void ExitGame()
    {
        if (isButtonPressed) return; // Cegah spamming tombol
        isButtonPressed = true;

        Application.Quit();
        Debug.Log("Exit Game");

        StartCoroutine(ResetButtonFlag());
    }

    private IEnumerator PlaySoundAndOpenPanel(int panelIndex)
    {
        AudioManager.Instance.PlaySFX("Mainmenu", 0); // Mainkan sound
        yield return new WaitForSeconds(0.2f); // Tunggu durasi sound (sesuaikan durasi ini)
        panels[1].SetActive(panelIndex == 1);
        panels[2].SetActive(panelIndex == 2);
        panels[3].SetActive(panelIndex == 3);
    }

    private IEnumerator ResetButtonFlag()
    {
        yield return new WaitForSeconds(0.5f); // Sesuaikan durasi ini jika diperlukan
        isButtonPressed = false; // Reset flag setelah durasi tertentu
    }

    public void bukaReferensi(){
        Application.OpenURL("https://www.notion.so/References-1f3061b95c7380419ee9ea5207fcd01a");
    }
}
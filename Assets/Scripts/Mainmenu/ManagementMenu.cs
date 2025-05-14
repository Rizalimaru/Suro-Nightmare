using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagementMenu : MonoBehaviour
{
    public GameObject[] panels; // isinya tampilan menu, options dan kredit
    private bool isButtonPressed = false; // Flag untuk mencegah spamming tombol

    public SceneController sceneController; // Referensi ke SceneController

    void Start()
    {
        panels[0].SetActive(true); // Menu utama aktif
        panels[1].SetActive(false); // Options tidak aktif
        panels[2].SetActive(false); // Kredit tidak aktif

        AudioManager.Instance.PlayBackgroundMusicWithTransition("Mainmenu", 0, 2f); // Memutar musik latar menu utama
    }

    void Update()
    {
        // Kalau panel indeks 1 atau 2 aktif, jika escape ditekan, kembali ke menu utama
        if (panels[1].activeSelf || panels[2].activeSelf)
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

    public void OpenKredit()
    {
        if (isButtonPressed) return; // Cegah spamming tombol
        isButtonPressed = true;

        StartCoroutine(PlaySoundAndOpenPanel(2));
        StartCoroutine(ResetButtonFlag());
    }

    public void BackToMenu()
    {
        panels[1].SetActive(false);
        panels[2].SetActive(false);
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
    }

    private IEnumerator ResetButtonFlag()
    {
        yield return new WaitForSeconds(0.5f); // Sesuaikan durasi ini jika diperlukan
        isButtonPressed = false; // Reset flag setelah durasi tertentu
    }
}
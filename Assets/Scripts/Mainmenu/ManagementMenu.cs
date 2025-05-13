using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagementMenu : MonoBehaviour
{
    public GameObject [] panels; // isinya tampilan menu, options dan kredit
    // Start is called before the first frame update
    void Start()
    {

        panels[0].SetActive(true); // Menu utama aktif
        panels[1].SetActive(false); // Options tidak aktif
        panels[2].SetActive(false); // Kredit tidak aktif

        AudioManager.Instance.PlayBackgroundMusicWithTransition("Mainmenu",0, 2f); // Memutar musik latar menu utama

        
    }

    // Update is called once per frame
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

    public void PlayGame(){
        // Load scene berikutnya
        AudioManager.Instance.PlaySFX("Mainmenu",0);
        AudioManager.Instance.StopBackgroundMusicWithTransition("Mainmenu", 1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Stage 1");

    }

    public void OpenOptions()
    {
        panels[1].SetActive(true);
        AudioManager.Instance.PlaySFX("Mainmenu",0);
    }

    public void OpenKredit()
    {
        AudioManager.Instance.PlaySFX("Mainmenu",0);
        panels[1].SetActive(false);
        panels[2].SetActive(true);
    }
    public void BackToMenu()
    {
        AudioManager.Instance.PlaySFX("Mainmenu",0);
        panels[1].SetActive(false);
        panels[2].SetActive(false);
    }

    public void ExitGame()
    {
        AudioManager.Instance.PlaySFX("Mainmenu",0);
        Application.Quit();
        Debug.Log("Exit Game");
    }

}

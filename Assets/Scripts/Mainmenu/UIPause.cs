using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Import SceneManagement untuk memuat scene

public class UIPause : MonoBehaviour
{
    public GameObject hideUI;
    public GameObject uipause;

    public playerController playerController;

    public Animator playerAnim;
    public PlayerItemData playerItemData;

    private bool restartGame;
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        // Ketika press esc akan pause
        if (Input.GetKeyDown(KeyCode.Escape) && restartGame == false)
        {
            if (uipause.activeSelf)
            {
                playerController.enabled = true;
                playerAnim.SetBool("isInteract", false);

                uipause.SetActive(false);
                hideUI.SetActive(true);
                Time.timeScale = 1f;
                
                float move = Input.GetAxisRaw("Horizontal");
                if (move != 0 && playerController.isGrounded && !playerController.isCrounching)
                {
                    string currentScene = SceneManager.GetActiveScene().name;

                    // Hentikan suara sebelumnya untuk memastikan suara baru diputar
                    AudioManager.Instance.StopSFX("PlayerMovement", 0);
                    AudioManager.Instance.StopSFX("PlayerMovement", 3);

                    // Putar suara berjalan berdasarkan arah baru
                    if (currentScene == "Stage 3")
                    {
                        AudioManager.Instance.PlaySFX("PlayerMovement", 3); // Play SFX untuk Stage3
                    }
                    else
                    {
                        AudioManager.Instance.PlaySFX("PlayerMovement", 0); // Play SFX default
                    }

                    playerController.isWalkingSoundPlaying = true; // Tandai bahwa suara berjalan sedang diputar
                }
            else
            {
                // Hentikan suara jika pemain tidak bergerak
                AudioManager.Instance.StopSFX("PlayerMovement", 0);
                AudioManager.Instance.StopSFX("PlayerMovement", 3);
                playerController.isWalkingSoundPlaying = false;
            }
            }
            else
            {
                AudioManager.Instance.StopSFX("PlayerMovement", 0);
                AudioManager.Instance.StopSFX("PlayerMovement", 3);
                AudioManager.Instance.StopSFX("PlayerMovement", 4);
                playerController.enabled = false;
                uipause.SetActive(true);
                hideUI.SetActive(false);
                Time.timeScale = 0f;
            }
        }

    }

    public void ResumeGame()
    {
        AudioManager.Instance.PlaySFX("Mainmenu", 0);
        float move = Input.GetAxisRaw("Horizontal");
        if (move != 0 && playerController.isGrounded && !playerController.isCrounching)
        {
            string currentScene = SceneManager.GetActiveScene().name;

            // Hentikan suara sebelumnya untuk memastikan suara baru diputar
            AudioManager.Instance.StopSFX("PlayerMovement", 0);
            AudioManager.Instance.StopSFX("PlayerMovement", 3);

            // Putar suara berjalan berdasarkan arah baru
            if (currentScene == "Stage 3")
            {
                AudioManager.Instance.PlaySFX("PlayerMovement", 3); // Play SFX untuk Stage3
            }
            else
            {
                AudioManager.Instance.PlaySFX("PlayerMovement", 0); // Play SFX default
            }

            playerController.isWalkingSoundPlaying = true; // Tandai bahwa suara berjalan sedang diputar
        }
        else
        {
            // Hentikan suara jika pemain tidak bergerak
            AudioManager.Instance.StopSFX("PlayerMovement", 0);
            AudioManager.Instance.StopSFX("PlayerMovement", 3);
            playerController.isWalkingSoundPlaying = false;
        }
        playerController.enabled = true;
        uipause.SetActive(false);
        hideUI.SetActive(true);
        Time.timeScale = 1f;
    }

    public void BackToMainMenu()
    {
        AudioManager.Instance.PlaySFX("Mainmenu", 0);
        restartGame = true;
        AudioManager.Instance.StopBackgroundMusicWithTransition("Stage1", 1f);
        AudioManager.Instance.StopBackgroundMusicWithTransition("Stage2", 1f);
        AudioManager.Instance.StopBackgroundMusicWithTransition("Stage3", 1f);
        playerItemData.isTutorialDone = false;
        playerController.enabled = true;
        uipause.SetActive(false);
        Time.timeScale = 1f;
        // Load Main Menu Scene
        SceneController.instance.LoadScene("Mainmenu");
    }

    // Restart scene ini 
    public void RestartGame()
    {
        AudioManager.Instance.PlaySFX("Mainmenu", 0);
        restartGame = true;
        playerController.enabled = true;
        uipause.SetActive(false);
        Time.timeScale = 1f;
        SceneController.instance.LoadScene(SceneManager.GetActiveScene().name);
    }

    



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Import SceneManagement untuk memuat scene

public class UIPause : MonoBehaviour
{

    public GameObject uipause;

    public playerController playerController;

    public Animator playerAnim;
    public PlayerItemData playerItemData;
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        // Ketika press esc akan pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (uipause.activeSelf)
            {
                playerController.enabled = true;
                playerAnim.SetBool("isInteract", false);

                uipause.SetActive(false);
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
                Time.timeScale = 0f;
            }
        }

    }

    public void ResumeGame()
    {
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
        Time.timeScale = 1f;
    }

    public void BackToMainMenu()
    {
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
        playerController.enabled = true;
        uipause.SetActive(false);
        Time.timeScale = 1f;
        SceneController.instance.LoadScene(SceneManager.GetActiveScene().name);
    }

    



}

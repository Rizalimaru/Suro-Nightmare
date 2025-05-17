using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Import SceneManagement untuk memuat scene

public class UIPause : MonoBehaviour
{

    public GameObject uipause;

    public playerController playerController;

    public Animator playerAnim;
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
                playerController.enabled =true;
                playerAnim.SetBool("isInteract", false);

                uipause.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                playerController.enabled = false;
                uipause.SetActive(true);
                Time.timeScale = 0f;
            }
        }

    }

    public void ResumeGame()
    {
        playerController.enabled = true;
        uipause.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BackToMainMenu()
    {
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

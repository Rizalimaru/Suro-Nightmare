using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Import SceneManagement untuk memuat scene

public class SceneController : MonoBehaviour
{

    public static SceneController instance; // Instance dari SceneController
    [SerializeField] Animator animatorSceneTransition;
    public string sceneToLoad;


    public void LoadScene(string sceneName)
    {
        sceneToLoad = sceneName; // Set nama scene yang akan dimuat
        StartCoroutine(StartSceneTransition(sceneToLoad)); // Panggil coroutine untuk transisi scene
    }

    // Fungsi ini dipanggil untuk memulai transisi ke scene baru
    public IEnumerator StartSceneTransition(string sceneName)
    {
        animatorSceneTransition.SetTrigger("SceneEnd"); // Trigger animasi transisi

        yield return new WaitForSeconds(3f); // Tunggu selama 1 detik sebelum memuat scene baru
        SceneManager.LoadSceneAsync(sceneToLoad); // Muat scene baru
        animatorSceneTransition.SetTrigger("SceneStart");
    }

    public void NextScene()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        animatorSceneTransition.SetTrigger("SceneEnd");
        yield return new WaitForSeconds(1f); // Tunggu selama 1 detik sebelum memuat scene baru
        SceneManager.LoadSceneAsync(sceneToLoad); // Muat scene baru
        animatorSceneTransition.SetTrigger("SceneStart");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            NextScene();
        }
    }


}

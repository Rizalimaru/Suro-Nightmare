using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Import SceneManagement untuk memuat scene

public class SceneController : MonoBehaviour
{

    public static SceneController instance; // Instance dari SceneController
    [SerializeField] Animator animatorSceneTransition;
    public string sceneToLoad;

    public void Start()
    {
        // Periksa apakah instance sudah ada
        if (instance == null)
        {
            instance = this; // Set instance jika belum ada
            //DontDestroyOnLoad(gameObject); // Jangan hancurkan objek ini saat memuat scene baru
        }

    }


    public void LoadScene(string sceneName)
    {
        sceneToLoad = sceneName; // Set nama scene yang akan dimuat
        StartCoroutine(StartSceneTransition(sceneToLoad)); // Panggil coroutine untuk transisi scene
    }

    // Fungsi ini dipanggil untuk memulai transisi ke scene baru
    public IEnumerator StartSceneTransition(string sceneName)
    {

        if(sceneToLoad == "Stage 2"){
            AudioManager.Instance.StopBackgroundMusicWithTransition("Stage1", 0);
        }

    
            
        animatorSceneTransition.SetTrigger("SceneEnd"); // Trigger animasi transisi

        yield return new WaitForSeconds(1f); // Tunggu selama 1 detik sebelum memuat scene baru
        SceneManager.LoadSceneAsync(sceneToLoad); // Muat scene baru

        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == sceneToLoad);
        animatorSceneTransition.SetTrigger("SceneStart");
    }

    public void NextScene()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        animatorSceneTransition.SetTrigger("SceneEnd");
        yield return new WaitForSeconds(3f); // Tunggu selama 1 detik sebelum memuat scene baru
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

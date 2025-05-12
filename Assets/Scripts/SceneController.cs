using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Import SceneManagement untuk memuat scene

public class SceneController : MonoBehaviour
{
    [SerializeField] Animator animatorSceneTransition;
    public string sceneToLoad;
    private void Awake()
    {
        // Pastikan objek ini tidak dihancurkan saat pergantian scene
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            NextScene();
        }
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


}

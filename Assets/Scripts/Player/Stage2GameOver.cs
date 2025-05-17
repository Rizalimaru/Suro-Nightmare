using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class Stage2GameOver : MonoBehaviour
{
    // GameOver Logic
    public GameObject gameOverUI; // Referensi ke UI Game Over
    public Animator gameOverAnimator; // Referensi ke Animator untuk Game Over
    public playerController playerController; // Referensi ke skrip playerController
    public GameObject tombolUlang; // Referensi ke tombol Ulang
    public UIPause uipause; // Referensi ke UI Pause
    public bool isGameOver = false; // Cegah pemanggilan GameOver berkali-kali

    // Start is called before the first frame update
    void Start()
    {
        gameOverAnimator = gameOverUI.GetComponent<Animator>();
    }

    void Update()
    {

    }

    // Tambahan: Deteksi tabrakan dengan player
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isGameOver && collision.collider.CompareTag("Pocong"))
        {
            
            isGameOver = true;
            StartCoroutine(GameOver());
        }
    }

    public IEnumerator GameOver()
    {
        uipause.enabled = false;
        tombolUlang.SetActive(false);
        gameOverUI.SetActive(true);
        gameOverAnimator.SetTrigger("gameOver");
        yield return new WaitForSeconds(2f); // Tunggu 2 detik sebelum menampilkan Game Over
        tombolUlang.SetActive(true);
        AudioListener.volume = 0;
        isGameOver = false;
        uipause.enabled = true;   
    }
}

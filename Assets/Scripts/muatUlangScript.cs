using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class muatUlangScript : MonoBehaviour
{
    public GameObject checkPoint;
    public GameObject player;
    public GameObject UIGameOver;

    public UIPause uipause;

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        StartCoroutine(muatUlang());
    }

    public void muatUlangGame()
    {
        Time.timeScale = 1f;
        StartCoroutine(muatUlang());
    }


    IEnumerator muatUlang()
    {
        
        player.transform.position =  checkPoint.transform.position;
        AudioListener.volume = 1f;
        ;
        yield return new WaitForSeconds(0.5f);
        uipause.enabled = true;
        UIGameOver.SetActive(false);

    }
}

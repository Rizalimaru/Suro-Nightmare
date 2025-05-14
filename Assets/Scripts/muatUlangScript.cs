using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class muatUlangScript : MonoBehaviour
{
    public GameObject checkPoint;
    public GameObject player;
    public GameObject UIGameOver;



    public void muatUlang()
    {
        player.transform.position =  checkPoint.transform.position;
        AudioListener.volume = 1f;
        UIGameOver.SetActive(false);
        Time.timeScale = 1f;

    }
}

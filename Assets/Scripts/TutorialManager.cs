using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialUI;
    public GameObject Turotial1;
    public GameObject Turotial2;
    public GameObject Turotial3;
    public PlayerItemData playerItemData;

    void Start()
    {   
        if (playerItemData.isTutorialDone == false)
        {
            StartCoroutine(ShowTutorial());
        }
        ;
    }

    IEnumerator ShowTutorial()
    {
        tutorialUI.SetActive(true);
        Turotial1.SetActive(true);
        yield return new WaitForSeconds(5f);
        Turotial1.SetActive(false);
        Turotial2.SetActive(true);
        yield return new WaitForSeconds(5f);
        Turotial2.SetActive(false);
        Turotial3.SetActive(true);
        yield return new WaitForSeconds(5f);
        Turotial3.SetActive(false);
        tutorialUI.SetActive(false);
        playerItemData.isTutorialDone = true; // Set isTutorialDone ke true setelah tutorial selesai
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class progressObjektif : MonoBehaviour
{
    public TMP_Text Objectif;
    public GameObject objectifHint;
    public GameObject nextStep;
    public GameObject TP;
    public int currentProgress;
    public int totalProgress;

    // Start is called before the first frame update
    void Start()
    {
        totalProgress = GameObject.FindGameObjectsWithTag("Menyan").Length;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateProgress();
        CheckProgress();
    }

    public void AddProgress()
    {
        currentProgress++;
        UpdateProgress();
    }

    public void UpdateProgress()
    {
        Objectif.text = "Matikan Menyan (" + currentProgress + "/" + totalProgress + ")"; 
    }

    public void CheckProgress()
    {
        if(currentProgress == totalProgress)
        {
            objectifHint.SetActive(false);
            nextStep.SetActive(true);
            TP.SetActive(true);
        }
    }
}

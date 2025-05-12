using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class objectInteraction : MonoBehaviour
{
    public GameObject interactionText;
    public Image barProgress;
    private bool isInteracting = false;

    void Start()
    {
        interactionText.SetActive(false);
    }

    void Update()
    {
        if(isInteracting)
        {
            if (Input.GetKey(KeyCode.E))
            {
                barProgress.fillAmount += Time.deltaTime / 2; // Mengisi progress bar

            }

            if (barProgress.fillAmount >= 1)
            {
                // Lakukan aksi saat progress bar penuh
                Debug.Log("Interaksi selesai!");
                barProgress.fillAmount = 0; // Reset progress bar
                isInteracting = false;
                interactionText.SetActive(false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {   
            if(isInteracting ==  false)
            {
                interactionText.SetActive(true);
            }else
            {
                interactionText.SetActive(false);
            }
            isInteracting = true; // Set isInteracting ke true saat pemain memasuki trigger
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactionText.SetActive(false);
            isInteracting = false; // Set isInteracting ke false saat pemain keluar dari trigger
        }
    }
}

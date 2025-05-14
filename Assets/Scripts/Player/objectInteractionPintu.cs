using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class objectInteraction : MonoBehaviour
{
    public GameObject interactionText;
    public Image barProgress;
    private bool isInteracting = false;
    public Sprite pintuBuka;
    private Animator playerAnim;
    public GameObject barier;

    void Start()
    {
        interactionText.SetActive(false);
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    void Update()
    {
        if(isInteracting)
        {
            if (Input.GetKey(KeyCode.E))
            {
                barProgress.fillAmount += Time.deltaTime / 7; // Mengisi progress bar;
            }

            if(Input.GetKeyDown(KeyCode.E))
            {   
                AudioManager.Instance.PlaySFX("InteractionObject", 1);
                playerAnim.SetBool("isInteract", true);
                interactionText.SetActive(false);
            }

            if(Input.GetKeyUp(KeyCode.E))
            {   
                AudioManager.Instance.StopSFX("InteractionObject", 1);
                playerAnim.SetBool("isInteract", false);
                interactionText.SetActive(true);
            }

            if (barProgress.fillAmount >= 1)
            {   
                // Lakukan aksi saat progress bar penuh
                Debug.Log("Interaksi selesai!");
                barProgress.fillAmount = 0; // Reset progress bar
                isInteracting = false;
                interactionText.SetActive(false);
                barier.SetActive(false); // Menonaktifkan objek barier
                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                AudioManager.Instance.PlaySFX("InteractionObject", 0);
                AudioManager.Instance.StopSFX("InteractionObject", 1);
                playerAnim.SetBool("isInteract", false);
                if(spriteRenderer != null)
                {
                    spriteRenderer.sprite = pintuBuka; // Ganti sprite pintu
                }else
                {
                    Debug.LogWarning("SpriteRenderer tidak ditemukan pada GameObject ini.");
                }
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

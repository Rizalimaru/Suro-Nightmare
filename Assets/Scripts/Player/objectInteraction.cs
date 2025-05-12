using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectInteraction : MonoBehaviour
{
    public GameObject interactionText;

    void Start()
    {
        interactionText.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactionText.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactionText.SetActive(false);
        }
    }
}

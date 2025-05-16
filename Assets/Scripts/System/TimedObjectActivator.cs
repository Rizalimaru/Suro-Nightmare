using System.Collections;
using UnityEngine;

public class TimedObjectActivator : MonoBehaviour
{
    [Header("Durasi tampil (detik)")]
    [SerializeField] private float duration = 2f;

    [Header("Objek berikutnya yang akan diaktifkan")]
    [SerializeField] private GameObject nextObject;

    void OnEnable()
    {
        StartCoroutine(ActivateNextAfterDelay());
    }

    IEnumerator ActivateNextAfterDelay()
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);

        if (nextObject != null)
        {
            nextObject.SetActive(true);
        }
    }
}

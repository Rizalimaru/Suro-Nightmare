using UnityEngine;

public class EnableKerisEffectOnDestroy : MonoBehaviour
{
    public kerisEffect targetKerisEffect; // Referensi ke script kerisEffect di GameObject lain

    private void OnDestroy()
    {
        if (targetKerisEffect != null)
        {
            targetKerisEffect.enabled = true;
            Debug.Log("kerisEffect telah diaktifkan setelah objek ini dihancurkan.");
        }
        else
        {
            Debug.LogWarning("Referensi ke kerisEffect belum di-assign.");
        }
    }
}

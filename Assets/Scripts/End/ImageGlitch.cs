using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageGlitch : MonoBehaviour
{
    public Image[] glitchImages;      // Drag 5 UI Image (berwarna transparan) ke sini
    public float glitchDuration = 11f; // Lama glitch
    public float glitchInterval = 0.1f; // Waktu antar glitch muncul

    private void Start()
    {

    }

    // jika objek ini diaktifkan, mulai glitch
    private void OnEnable()
    {
        StartCoroutine(PlayGlitch());
    }

    IEnumerator PlayGlitch()
    {
        float timer = 0f;

        // Pastikan semua gambar mulai dalam kondisi off
        foreach (Image img in glitchImages)
        {
            img.enabled = false;
        }

        while (timer < glitchDuration)
        {
            // Matikan semua gambar
            foreach (Image img in glitchImages)
                img.enabled = false;

            // Acak salah satu gambar untuk ditampilkan
            int randomIndex = Random.Range(0, glitchImages.Length);
            glitchImages[randomIndex].enabled = true;

            // Tunggu sebentar
            yield return new WaitForSeconds(glitchInterval);

            timer += glitchInterval;
        }

        // Matikan semua gambar setelah selesai
        foreach (Image img in glitchImages)
        {
            img.enabled = false;
        }
    }
}

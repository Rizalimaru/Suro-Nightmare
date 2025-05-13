using UnityEngine;

public class Glitch : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float glitchDuration = 0.1f;
    public float glitchInterval = 1f;

    private Vector3 originalPosition;
    private Color originalColor;

    void Start()
    {
        originalPosition = transform.localPosition;
        originalColor = spriteRenderer.color;
        InvokeRepeating("StartGlitch", glitchInterval, glitchInterval);
    }

    void StartGlitch()
    {
        StartCoroutine(DoGlitch());
    }

    System.Collections.IEnumerator DoGlitch()
    {
        // Glitch Posisi & Warna
        spriteRenderer.color = Color.magenta;
        transform.localPosition = originalPosition + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.05f, 0.05f), 0);
        yield return new WaitForSeconds(glitchDuration);
        spriteRenderer.color = originalColor;
        transform.localPosition = originalPosition;
    }
}

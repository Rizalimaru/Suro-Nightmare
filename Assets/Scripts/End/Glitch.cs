using UnityEngine;

public class GlitchScript2D : MonoBehaviour
{
    public SpriteRenderer baseSprite;
    public float glitchInterval = 1.5f;
    public float glitchDuration = 0.2f;
    public float cameraShakeIntensity = 0.1f;

    private GameObject redLayer, greenLayer, blueLayer;
    private Color baseColor;
    private Vector3 originalPos;
    private Transform cam;

    void Start()
    {
        originalPos = transform.localPosition;
        baseColor = baseSprite.color;

        cam = Camera.main.transform;
        CreateRGBLayers();
        InvokeRepeating(nameof(TriggerGlitch), glitchInterval, glitchInterval);
    }

    void CreateRGBLayers()
    {
        redLayer = CreateLayer("R", new Color(1, 0, 0, 0.5f));
        greenLayer = CreateLayer("G", new Color(0, 1, 0, 0.5f));
        blueLayer = CreateLayer("B", new Color(0, 0, 1, 0.5f));
    }

    GameObject CreateLayer(string name, Color tint)
    {
        GameObject obj = new GameObject(name);
        obj.transform.parent = transform;
        obj.transform.localPosition = Vector3.zero;
        SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = baseSprite.sprite;
        sr.sortingOrder = baseSprite.sortingOrder + 1;
        sr.color = tint;
        return obj;
    }

    void TriggerGlitch()
    {
        StartCoroutine(DoGlitch());
    }

    System.Collections.IEnumerator DoGlitch()
    {
        // Simpan posisi awal kamera
        Vector3 camOriginalPos = cam.position;

        // Geser sprite
        transform.localPosition = originalPos + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.05f, 0.05f), 0);

        // Geser RGB layer sedikit
        redLayer.transform.localPosition = new Vector3(Random.Range(-0.03f, 0.03f), 0, 0);
        greenLayer.transform.localPosition = new Vector3(Random.Range(0.03f, 0.05f), 0, 0);
        blueLayer.transform.localPosition = new Vector3(Random.Range(-0.05f, -0.03f), 0, 0);

        // Flicker transparan
        baseSprite.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0.4f);

        // Camera shake
        cam.position += new Vector3(Random.Range(-cameraShakeIntensity, cameraShakeIntensity), Random.Range(-cameraShakeIntensity, cameraShakeIntensity), 0);

        yield return new WaitForSeconds(glitchDuration);

        // Reset semua
        transform.localPosition = originalPos;
        redLayer.transform.localPosition = Vector3.zero;
        greenLayer.transform.localPosition = Vector3.zero;
        blueLayer.transform.localPosition = Vector3.zero;
        baseSprite.color = baseColor;
        cam.position = camOriginalPos;
    }
}

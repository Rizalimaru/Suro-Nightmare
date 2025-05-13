using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parralaxBackground : MonoBehaviour
{
    public Transform cameraTransform; // Referensi ke kamera utama
    public Vector2 parallaxEffectMultiplier; // Kecepatan parallax untuk X dan Y
    public Vector2 constantMovementSpeed; // Kecepatan gerakan konstan untuk background

    private Vector3 lastCameraPosition;
    private float textureUnitSizeX; // Ukuran lebar tekstur background
    private float textureUnitSizeY; // Ukuran tinggi tekstur background (opsional)

    void Start()
    {
        // Simpan posisi awal kamera
        lastCameraPosition = cameraTransform.position;

        // Hitung ukuran tekstur berdasarkan scale objek
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit * transform.localScale.x;
        textureUnitSizeY = texture.height / sprite.pixelsPerUnit * transform.localScale.y; // Opsional
    }

    void LateUpdate()
    {
        // Hitung perbedaan posisi kamera
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        // Gerakkan background berdasarkan perbedaan posisi kamera dan multiplier
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y, 0);

        // Tambahkan gerakan konstan
        transform.position += new Vector3(constantMovementSpeed.x * Time.deltaTime, constantMovementSpeed.y * Time.deltaTime, 0);

        // Perbarui posisi terakhir kamera
        lastCameraPosition = cameraTransform.position;

        // Periksa apakah background perlu diulang
        if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
        {
            float offsetX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x + offsetX, transform.position.y, transform.position.z);
        }

        // Opsional: Periksa untuk sumbu Y jika diperlukan
        if (Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureUnitSizeY)
        {
            float offsetY = (cameraTransform.position.y - transform.position.y) % textureUnitSizeY;
            transform.position = new Vector3(transform.position.x, cameraTransform.position.y + offsetY, transform.position.z);
        }
    }
}

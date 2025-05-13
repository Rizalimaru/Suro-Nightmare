using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public playerController playerController; // Referensi ke skrip playerController
    public Camera cam; // Kamera utama
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    public float leftBoundary; // Batas kiri kamera
    public float rightBoundary; // Batas kanan kamera

    private Vector3 defaultOffset; // Offset default kamera
    private float defaultOrthographicSize = 5f; // Ukuran default kamera ortogonal
    private float crouchOrthographicSize = 3f; // Ukuran kamera saat crouch
    private Vector3 velocity = Vector3.zero; // Kecepatan untuk SmoothDamp

    private void Start()
    {
        cam = Camera.main;
        defaultOrthographicSize = cam.orthographicSize; // Simpan ukuran default kamera
        defaultOffset = offset; // Simpan offset default
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Pergerakan kamera mengikuti pemain
        Vector3 desiredPosition = target.position + offset;

        // Batasi pergerakan kamera di dalam batas kiri dan kanan
        if (desiredPosition.x < leftBoundary)
        {
            desiredPosition.x = leftBoundary;
        }
        else if (desiredPosition.x > rightBoundary)
        {
            desiredPosition.x = rightBoundary;
        }

        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        transform.position = smoothedPosition;

        // Ubah ukuran kamera berdasarkan status crouch pemain
        if (playerController != null)
        {
            if (playerController.isCrounching)
            {   
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, crouchOrthographicSize, Time.deltaTime * 5f);
                offset = new Vector3(0, 0, -10); // Ubah offset saat crouch
            }
            else
            {
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, defaultOrthographicSize, Time.deltaTime * 5f);
                offset = defaultOffset; // Kembalikan offset ke default
            }
        }
    }
}

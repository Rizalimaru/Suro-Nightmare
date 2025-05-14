using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class enemyGenderuwo : MonoBehaviour
{
    public float patrolSpeed = 2f; // Kecepatan jalan musuh
    public float leftPatrolLimit; // Batas kiri patrol
    public float rightPatrolLimit; // Batas kanan patrol

    private Rigidbody2D rb;
    private bool isFacingRight = true;

    // Audio
    public AudioSource stepSound; // AudioSource untuk suara langkah
    public AudioSource heartbeatSound; // AudioSource untuk suara heartbeat
    private bool isStepSoundPlaying = false; // Melacak status suara langkah

    //GameOve Logic
    public GameObject gameOverUI; // Referensi ke UI Game Over
    public Animator gameOverAnimator; // Referensi ke Animator untuk Game Over
    public playerController playerController; // Referensi ke skrip playerController

    // Heartbeat logic
    public Transform player; // Referensi ke pemain
    private float heartbeatBasePitch = 1f; // Pitch dasar untuk heartbeat
    private float heartbeatMaxPitch = 2f; // Pitch maksimum untuk heartbeat
    private float heartbeatDistanceThreshold = 10f; // Jarak maksimum untuk memulai heartbeat

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //isFacingRight = false;

        // Pastikan heartbeat diatur untuk loop
        if (heartbeatSound != null)
        {
            heartbeatSound.loop = true;
            heartbeatSound.Play();
            heartbeatSound.volume = 0; // Volume awal diatur ke 0
        }
        gameOverAnimator = gameOverUI.GetComponent<Animator>();
    }

    private void Update()
    {
        Patrol();
        UpdateHeartbeat();
    }

    private void Patrol()
    {
        float moveDirection = isFacingRight ? 1 : -1;
        rb.velocity = new Vector2(moveDirection * patrolSpeed, rb.velocity.y);

        // Mainkan suara langkah jika Genderuwo sedang bergerak
        if (!isStepSoundPlaying && stepSound != null)
        {
            stepSound.loop = true;
            stepSound.Play();
            isStepSoundPlaying = true;
        }

        // Berhenti dan balik arah jika mencapai batas patrol
        if (isFacingRight && transform.position.x >= rightPatrolLimit)
        {
            Flip();
        }
        else if (!isFacingRight && transform.position.x <= leftPatrolLimit)
        {
            Flip();
        }
    }

    private void UpdateHeartbeat()
    {
        if (heartbeatSound == null || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= heartbeatDistanceThreshold)
        {
            // Hitung pitch berdasarkan jarak (semakin dekat, semakin cepat)
            float normalizedDistance = Mathf.Clamp01(1 - (distanceToPlayer / heartbeatDistanceThreshold));
            heartbeatSound.pitch = Mathf.Lerp(heartbeatBasePitch, heartbeatMaxPitch, normalizedDistance);

            // Atur volume berdasarkan jarak (semakin dekat, semakin keras)
            heartbeatSound.volume = normalizedDistance;
        }
        else
        {
            // Jika pemain terlalu jauh, hentikan heartbeat
            heartbeatSound.volume = 0;
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        // Hentikan suara langkah saat Genderuwo berhenti untuk berbalik
        if (isStepSoundPlaying && stepSound != null)
        {
            stepSound.Stop();
            isStepSoundPlaying = false;
        }
    }

    IEnumerator GameOver()
    {   
        gameOverUI.SetActive(true);
        gameOverAnimator.SetTrigger("gameOver");
        yield return new WaitForSeconds(2f); // Tunggu 2 detik sebelum menampilkan Game Over
        AudioListener.volume = 0;
        Time.timeScale = 0; // Hentikan permainan
        //hentikan semua suara
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {   
            if(playerController.isCrounching == true)
            {
                // Jika Genderuwo menyentuh pemain yang sedang jongkok, tidak melakukan apa-apa
                return;
            }
            else
            {
                // Jika Genderuwo menyentuh pemain yang tidak jongkok, panggil GameOver
                StartCoroutine(GameOver());
            }
        }
    }
}

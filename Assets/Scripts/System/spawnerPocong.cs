using UnityEngine;

public class spawnerPocong : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject pocongPrefab;          // Prefab pocong yang ingin di-spawn
    public float spawnDistanceBehind = 2f;   // Seberapa jauh di belakang menyan spawnnya
    public float triggerDistance = 5f;       // Jarak player ke menyan agar spawn terjadi

    [Header("References")]
    public Transform player;                 // Referensi ke Transform player

    [HideInInspector]
    public GameObject spawnedPocong;         // Simpan referensi Pocong yang sudah di-spawn

    private bool hasSpawned = false;         // Mencegah spawn ulang

    void Update()
    {
        if (!hasSpawned && player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance < triggerDistance)
            {
                Vector3 spawnPos = transform.position - transform.right * spawnDistanceBehind;
                spawnedPocong = Instantiate(pocongPrefab, spawnPos, Quaternion.identity);
                hasSpawned = true;
            }
        }
    }

    // Jika kamu ingin spawn ulang di masa depan (opsional)
    public void ResetSpawner()
    {
        hasSpawned = false;
        spawnedPocong = null;
    }
}

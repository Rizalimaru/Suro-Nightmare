using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class objctIneraction : MonoBehaviour
{
    public GameObject canvasInteraction; // UI untuk interaksi
    public PlayerItemData playerItemData; // Data pemain
    public AquirepItemData aquirepItemData; // Data item yang diambil
    public Image itemIcon; // Ikon item
    public TextMeshProUGUI itemDescription; // Deskripsi item
    public GameObject canvasEToInteract;

    public kerisEffect kerisEffect;

    // Dropdown di Inspector untuk memilih item
    public enum ItemType { DapetKafan, DapetKetis, DapetKaca }
    [SerializeField] private ItemType selectedItem;

    private bool isPlayerInRange = false; // Menyimpan status apakah pemain berada di dalam trigger

    private void Start()
    {
        canvasInteraction.SetActive(false);
        canvasEToInteract.SetActive(false);
    }

    private void Update()
    {
        // Periksa input E hanya jika pemain berada di dalam trigger
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {   
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false; // Menyembunyikan sprite objek ini
            canvasEToInteract.SetActive(false); // Menyembunyikan canvas interaksi
            canvasInteraction.SetActive(true);
            UpdateItemData();
            UpdatePlayerItemData();
            StartCoroutine(HideCanvasAfterDelay(5f)); // Menyembunyikan canvas setelah 2 detik
        }
    }

    private void UpdatePlayerItemData()
    {
        // Reset semua boolean di PlayerItemData
        playerItemData.dapetKafan = false;
        playerItemData.dapetKeris = false;
        playerItemData.dapetKaca = false;

        // Set boolean berdasarkan pilihan dropdown di Inspector
        switch (selectedItem)
        {
            case ItemType.DapetKafan:
                playerItemData.dapetKafan = true;
                AudioManager.Instance.PlaySFX("PlayerMovement",1);
                break;
            case ItemType.DapetKetis:
                // Aktifkan efek keris biar gg bos 
                AudioManager.Instance.PlaySFX("Stage2", 0);
                kerisEffect.enabled = true;
                playerItemData.dapetKeris = true;
                break;
            case ItemType.DapetKaca:
                playerItemData.dapetKaca = true;
                break;
        }

        Debug.Log($"PlayerItemData updated: Kafan={playerItemData.dapetKafan}, Keris={playerItemData.dapetKeris}, Kaca={playerItemData.dapetKaca}");
    }

    public void UpdateItemData()
    {
        // Perbarui ikon dan deskripsi item berdasarkan AquirepItemData
        if (aquirepItemData != null)
        {
            itemIcon.sprite = aquirepItemData.itemIcon;
            itemDescription.text = aquirepItemData.itemDescription;
        }
    }

    private IEnumerator HideCanvasAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canvasInteraction.SetActive(false);
        Destroy(gameObject); // Hapus objek setelah interaksi
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true; // Tandai bahwa pemain berada di dalam trigger
            canvasEToInteract.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false; // Tandai bahwa pemain keluar dari trigger
            canvasEToInteract.SetActive(false);
        }
    }
}

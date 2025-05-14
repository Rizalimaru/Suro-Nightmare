using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class dialogSystemBlank1 : MonoBehaviour
{
    [Header("UI Komponen")]
    [SerializeField] private TMP_Text dialogText;

    [Header("Dialog")]
    [TextArea(3, 5)]
    [SerializeField] private string[] dialogLines;

    [Header("Typing Effect")]
    [SerializeField] private float typingSpeed = 0.03f;

    [Header("Aksi Selanjutnya")]
    [SerializeField] private GameObject nextGameObject;
    [SerializeField] private string nextSceneName;

    private int currentLine = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    public GameObject hitamtransisi;

    void Start()
    {
        ShowNextLine();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                dialogText.text = dialogLines[currentLine];
                isTyping = false;
                currentLine++;
            }
            else
            {
                ShowNextLine();
            }
        }
    }

    void ShowNextLine()
    {
        if (currentLine < dialogLines.Length)
        {
            typingCoroutine = StartCoroutine(TypeLine(dialogLines[currentLine]));
        }
        else
        {
            // Dialog selesai
            dialogText.text = "";
            hitamtransisi.SetActive(true); // Aktifkan objek hitamtransisi
            
            StartCoroutine(ShowNextGameObjectAfterDelay(2f)); // Tampilkan objek berikutnya setelah delay


        }
    }

    IEnumerator TypeLine(string dialog)
    {
        isTyping = true;
        dialogText.text = "";
 
        foreach (char letter in dialog)
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        currentLine++;
    }

    // Megnshow next game object after delay   
    private IEnumerator ShowNextGameObjectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false); // Atau aksi selanjutnya
        if (nextGameObject != null)
        {
            nextGameObject.SetActive(true); // Tampilkan objek berikutnya
        }
    }
}

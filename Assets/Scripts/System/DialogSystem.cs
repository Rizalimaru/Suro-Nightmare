using System.Collections;
using UnityEngine;
using TMPro;

public class dialogSystem : MonoBehaviour
{
    [Header("UI Komponen")]
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private TMP_Text nameText;

    [Header("Dialog")]
    [SerializeField] private string[] nameLines;
    [TextArea(3, 5)]
    [SerializeField] private string[] dialogLines;

    [Header("Typing Effect")]
    [SerializeField] private float typingSpeed = 0.03f;

    [Header("Aksi Selanjutnya")]
    [SerializeField] private GameObject nextGameObject; // Objek yang akan diaktifkan setelah dialog selesai

    private int currentLine = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

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
                nameText.text = nameLines[currentLine];
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
        if (currentLine < dialogLines.Length && currentLine < nameLines.Length)
        {
            typingCoroutine = StartCoroutine(TypeLine(dialogLines[currentLine], nameLines[currentLine]));
        }
        else
        {
            // Dialog selesai
            dialogText.text = "";
            nameText.text = "";
            gameObject.SetActive(false);

            if (nextGameObject != null)
            {
                nextGameObject.SetActive(true); // Tampilkan objek berikutnya
            }
        }
    }

    IEnumerator TypeLine(string dialog, string speaker)
    {
        isTyping = true;
        dialogText.text = "";
        nameText.text = speaker;

        foreach (char letter in dialog)
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        currentLine++;
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogSystem : MonoBehaviour
{
    [Header("UI Komponen")]
    [SerializeField] private TMP_Text dialogText;

    [Header("Dialog")]
    [TextArea(3, 5)]
    [SerializeField] private string[] dialogLines;

    [Header("Typing Effect")]
    [SerializeField] private float typingSpeed = 0.03f;

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
                // Skip typing and show full line instantly
                StopCoroutine(typingCoroutine);
                dialogText.text = dialogLines[currentLine - 1];
                isTyping = false;
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
            currentLine++;
        }
        else
        {
            // Dialog selesai, sembunyikan atau lanjutkan ke aksi berikutnya
            dialogText.text = "";
            gameObject.SetActive(false); // atau panggil event berikutnya
        }
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogText.text = "";

        foreach (char letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}

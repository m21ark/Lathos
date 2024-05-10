using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public TextMeshProUGUI text;
    public bool autoSkip = true;

    private float textSpeedDelay = 0.01f;
    private float nextBoxDelay = 2f;
    private int index = 0;
    private bool isTyping = false;
    private DialogueData dialogueData;
    [HideInInspector] public bool isActive = false;

    public void Display(DialogueData data)
    {
        if (isActive || data == null) return;
        dialogueData = data;
        gameObject.SetActive(true);
        text.text = "";
        index = 0;
        isActive = true;
        StartCoroutine(TypeLine());
    }

    void Update()
    {
        if (autoSkip) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!isTyping)
                NextLine(); // Show next line
            else if (text.text != dialogueData.dialoguePhrases[index])
            {
                // Skip text typing and show full line
                StopCoroutine(TypeLine());
                text.text = "";
                isTyping = false;
                text.text += dialogueData.dialoguePhrases[index];
            }
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        foreach (char c in dialogueData.dialoguePhrases[index].ToCharArray())
        {
            if (!isTyping) break;
            text.text += c;
            yield return new WaitForSecondsRealtime(textSpeedDelay);
        }
        isTyping = false;

        if (autoSkip)
        {
            yield return new WaitForSecondsRealtime(nextBoxDelay);
            NextLine();
        }
    }

    void NextLine()
    {
        if (index < dialogueData.dialoguePhrases.Count - 1)
        {
            index++;
            text.text = "";
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
            isActive = false;
        }
    }
}

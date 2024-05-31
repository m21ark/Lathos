using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DialogueController : MonoBehaviour
{
    public static DialogueController instance { get; private set; }
    public TextMeshProUGUI text;
    public GameObject dialogueBox;
    public bool autoSkip = true;

    // Private variables
    private float textSpeedDelay = 0.01f;
    private float nextBoxDelay = 2f;
    private int index = 0;
    private bool isTyping = false;
    private DialogueData dialogueData;
    private bool isActive = false;

    // Dialogue Data
    public List<DialogueData> dialogueDataList;

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("More than one DialogueController in the scene");
        else instance = this;
    }

    public void Display(DialogueData data)
    {
        if (isActive || data == null) return;
        dialogueData = data;
        text.text = "";
        index = 0;
        isActive = true;
        dialogueBox.SetActive(true);
        StartCoroutine(TypeLine());
    }

    void Update()
    {
        if (autoSkip) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!isTyping) NextLine(); // Show next line
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
            dialogueBox.SetActive(true);
            isActive = false;
        }
    }

    public void StartDialogue(string key)
    {
        DialogueData data = dialogueDataList.Find(data => data.dialogueName == key);
        if (data != null) instance.Display(data);
        else Debug.LogError("Dialogue data with key '" + key + "' not found.");
    }
}

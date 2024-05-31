using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DialogueController : MonoBehaviour
{
    public static DialogueController instance { get; private set; }
    public TextMeshProUGUI text;
    public GameObject dialogueBox;
    public bool isDialogueOver = false;

    // Private variables
    private float textSpeedDelay = 0.035f;
    private float nextBoxDelay = 4f;
    private int index = 0;
    private bool isActive = false;
    private DialogueData dialogueData;

    // Dialogue Data
    public List<DialogueData> dialogueDataList;

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("More than one DialogueController in the scene");
        else instance = this;
    }

    private void Display(DialogueData data)
    {
        if (isActive || data == null) return;
        dialogueData = data;
        text.text = "";
        index = 0;
        isActive = true;
        dialogueBox.SetActive(true);
        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        foreach (char c in dialogueData.dialoguePhrases[index].ToCharArray())
        {
            text.text += c;
            yield return new WaitForSecondsRealtime(textSpeedDelay);
        }

        yield return new WaitForSecondsRealtime(nextBoxDelay);
        NextLine();
    }

    private void NextLine()
    {
        if (index < dialogueData.dialoguePhrases.Count - 1)
        {
            index++;
            text.text = "";
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueBox.SetActive(false);
            isActive = false;
            isDialogueOver = true;
        }
    }

    public void StartDialogue(string key)
    {
        isDialogueOver = false;
        DialogueData data = dialogueDataList.Find(data => data.dialogueName == key);
        if (data != null) instance.Display(data);
        else Debug.LogError("Dialogue data with key '" + key + "' not found.");
    }

    public void StartOpeningWithSound(string key){
        AudioManager.instance.PlayOpening(int.Parse(key[key.Length - 1].ToString()));
        StartDialogue(key);
    }
}

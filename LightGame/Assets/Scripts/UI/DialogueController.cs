using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public TextMeshProUGUI text;
    public string[] lines;
    public bool autoSkip = true;

    private float textSpeedDelay = 0.01f;
    private float nextBoxDelay = 2f;

    private int index = 0;
    private bool isTyping = false;

    public void Display(){
        gameObject.SetActive(true);
        text.text = "";
        index = 0;
        StartCoroutine(TypeLine());
    }

    void Update()
    {
        if(autoSkip) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!isTyping)
                NextLine(); // Show next line
            else if (text.text != lines[index])
            {
                // Skip text typing and show full line
                StopCoroutine(TypeLine());
                text.text = "";
                isTyping = false;
                text.text += lines[index];
            }
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        foreach (char c in lines[index].ToCharArray())
        {
            if(!isTyping) break;
            text.text += c;
            yield return new WaitForSeconds(textSpeedDelay);
        }
        isTyping = false;

        if (autoSkip){
            yield return new WaitForSeconds(nextBoxDelay);
            NextLine();
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            text.text = "";
            StartCoroutine(TypeLine());
        }
        else gameObject.SetActive(false);
    }
}

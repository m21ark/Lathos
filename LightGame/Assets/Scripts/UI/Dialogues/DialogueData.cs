using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewDialogueData", menuName = "Dialogue Data")]
public class DialogueData : ScriptableObject{
    public string dialogueName;
    [TextArea(3, 10)] public List<string> dialoguePhrases = new List<string>();
}

using UnityEngine;

public class SorcererTag : MonoBehaviour
{
    public int stackCounter = 1;

    // on init, create a text game object on top of the target with the stack counter
    void Start()
    {
        GameObject stackText = new GameObject("StackText");
        stackText.transform.SetParent(this.transform);
        stackText.transform.position = this.transform.position + new Vector3(0, 1, 0);
        stackText.transform.rotation = Camera.main.transform.rotation;
        stackText.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        TextMesh textMesh = stackText.AddComponent<TextMesh>();
        textMesh.text = stackCounter.ToString();
        textMesh.fontSize = 500;
        textMesh.color = Color.red;
        textMesh.characterSize = 0.1f;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
    }

    public void addStack(int stackCounter = 1){
        this.stackCounter += stackCounter;
        if(this.transform.Find("StackText") != null){
        TextMesh textMesh = this.transform.Find("StackText").GetComponent<TextMesh>();
        if (textMesh != null)
            textMesh.text = this.stackCounter.ToString();
        }
    }

    // on destroy, remove the stack counter text
    void OnDestroy()
    {
        Destroy(this.transform.Find("StackText").gameObject);
    }
}
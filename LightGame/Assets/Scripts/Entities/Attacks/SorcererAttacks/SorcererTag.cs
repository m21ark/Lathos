using UnityEngine;
using UnityEngine.VFX;

public class SorcererTag : MonoBehaviour
{   
    public VisualEffect VFXStacks;
    public VisualEffect VFXExplosion;
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

        // Create a new Visual Effect Component
        VFXStacks = Instantiate(VFXStacks, this.transform.position, Quaternion.identity);

        // Send an event to the VFX to start the stack animation
        VFXStacks.SendEvent("OnStack");
    }

    private void FixedUpdate() {
        VFXStacks.transform.position = this.transform.position;
    }

    public void addStack(int stackCounter = 1)
    {
        this.stackCounter += stackCounter;
        this.stackCounter = Mathf.Clamp(this.stackCounter, 1, 5); // limit the stack counter to 5

        if (this.transform.Find("StackText") != null)
        {
            TextMesh textMesh = this.transform.Find("StackText").GetComponent<TextMesh>();
            if (textMesh != null)
                VFXStacks.SendEvent("OnStack");
                textMesh.text = this.stackCounter.ToString();
        }
    }

    // on destroy, remove the stack counter text
    void OnDestroy()
    {
        VFXExplosion = Instantiate(VFXExplosion, this.transform.position, Quaternion.identity);
        VFXExplosion.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        Destroy(VFXExplosion.gameObject, 2.0f);
        Destroy(VFXStacks.gameObject);
        Destroy(this.transform.Find("StackText").gameObject);
    }
}
using UnityEngine;
using UnityEngine.VFX;

public class SorcererTag : MonoBehaviour
{   
    public VisualEffect VFXStacks;
    public VisualEffect VFXExplosion;
    public int stackCounter = 1;
    private int vfxStackCounter = 1;

    // on init, create a text game object on top of the target with the stack counter
    void Start()
    {
        Vector3 pos = this.transform.position;
        if (this.gameObject.tag == "Boss") {
            pos.y += 12.0f;
        }
        // Create a new Visual Effect Component
        VFXStacks = Instantiate(VFXStacks, pos, Quaternion.identity);

        // Set the parent of the VFX
        VFXStacks.transform.SetParent(this.transform);
    }

    // It was necessary to use FixedUpdate to update the stack counter, because the VFX Event is not triggered immediately
    // If I try to send multiple events in the same frame, the VFX will only trigger the last one
    private void FixedUpdate() {
        if (vfxStackCounter > 0) {
            vfxStackCounter -= 1;
            VFXStacks.SendEvent("OnStack");
        }
    }

    public void addStack(int stackCounter = 1)
    {   
        int old = this.stackCounter;
        this.stackCounter += stackCounter;
        this.stackCounter = Mathf.Clamp(this.stackCounter, 1, 5); // limit the stack counter to 5
        vfxStackCounter += this.stackCounter - old;
    }

    // on destroy, remove the stack counter text
    void OnDisable()
    {
        if (!gameObject.scene.isLoaded) return;
        VFXExplosion = Instantiate(VFXExplosion, this.transform.position, Quaternion.identity);
        VFXExplosion.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        Destroy(VFXExplosion.gameObject, 2.0f);
        Destroy(VFXStacks.gameObject);
    }
}
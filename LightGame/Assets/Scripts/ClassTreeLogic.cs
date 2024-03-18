using UnityEngine;
using UnityEngine.UI;

public class ClassTreeLogic : MonoBehaviour
{
    public GameObject classSelectObj;

    public void selectClassA()
    {
        Debug.Log("Selected Class A");
        ChangePlayerColor(Color.red);
    }

    public void selectClassB()
    {
        Debug.Log("Selected Class B");
        ChangePlayerColor(Color.blue);
    }

    public void selectClassC()
    {
        Debug.Log("Selected Class C");
        ChangePlayerColor(Color.green);
    }

    private void ChangePlayerColor(Color color){
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Renderer playerRenderer = player.GetComponent<Renderer>();
        playerRenderer.material.color = color;
    }
}

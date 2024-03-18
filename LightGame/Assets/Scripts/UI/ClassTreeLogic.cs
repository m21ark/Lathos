using UnityEngine;
using UnityEngine.UI;

public class ClassTreeLogic : MonoBehaviour
{
    public GameObject classSelectMenuObj;
    
    private GameLogic gameLogic;

    void Start(){
        gameLogic = GameObject.FindWithTag("GameController").GetComponent<GameLogic>();
    }
    
    public void Update(){

        // Check if E is pressed to invoke class selection
        if (Input.GetKeyDown(KeyCode.E))
            ToggleClassSelectMenu();
    }

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

        // Remove menu after choice is made
        ToggleClassSelectMenu();
    }

    public void ToggleClassSelectMenu(){
        if(gameLogic == null) gameLogic = GameObject.FindWithTag("GameController").GetComponent<GameLogic>();

        if (gameLogic.isPaused){                    
            // Hide Class Selection Menu
            gameLogic.toggleCursor(false);

            Time.timeScale = 1f;
            gameLogic.isPaused = false;
            if(classSelectMenuObj != null)
                classSelectMenuObj.SetActive(false);
        }
        else{
            // Show Class Selection Menu
            gameLogic.toggleCursor(true);
            
            Time.timeScale = 0f;
            gameLogic.isPaused = true;
            if(classSelectMenuObj != null)
                classSelectMenuObj.SetActive(true);
        }
    }
}

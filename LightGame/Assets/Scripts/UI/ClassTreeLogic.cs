using UnityEngine;
using UnityEngine.UI;

public class ClassTreeLogic : MonoBehaviour
{
    public GameObject classSelectMenuObj;
    private GameLogic gameLogic;
    public bool isSelecting = false;

    // Class Objects
    public GameObject classAPrefab; 
    public GameObject classBPrefab;
    public GameObject classCPrefab;

    void Start(){
        gameLogic = GameObject.FindWithTag("GameController").GetComponent<GameLogic>();
    }
    
    public void SelectClassA()
    {
        Debug.Log("Selected Class A");
        ReplacePlayer(classAPrefab);
    }

    public void SelectClassB()
    {
        Debug.Log("Selected Class B");
        ReplacePlayer(classBPrefab);
    }

    public void SelectClassC()
    {
        Debug.Log("Selected Class C");
        ReplacePlayer(classCPrefab);
    }

    private void ReplacePlayer(GameObject newPlayerPrefab)
    {
        // Get old player info
        GameObject oldPlayer = GameObject.FindGameObjectWithTag("Player");
        Transform pivot = oldPlayer.transform.Find("CameraPivot").transform;
        Vector3 oldPlayerPosition = oldPlayer.transform.position;
 
        // Set old player rotation to have x-axis and z-axis rotation as 0
        Quaternion oldPlayerRotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);

        // Generate the new player
        GameObject newPlayer = Instantiate(newPlayerPrefab, oldPlayerPosition, oldPlayerRotation);

        // Remove old player
        Destroy(oldPlayer.transform.parent.gameObject);

        // Remove menu after choice is made
        ToggleClassSelectMenu();
    }

    public void ToggleClassSelectMenu(){
        if(gameLogic == null) gameLogic = GameObject.FindWithTag("GameController").GetComponent<GameLogic>();

        if (gameLogic.isPaused){                    
            // Hide Class Selection Menu
            gameLogic.toggleCursor(false);
            isSelecting = false;

            Time.timeScale = 1f;
            gameLogic.isPaused = false;
            if(classSelectMenuObj != null)
                classSelectMenuObj.SetActive(false);
        }
        else{
            // Show Class Selection Menu
            gameLogic.toggleCursor(true);
            isSelecting = true;
            
            Time.timeScale = 0f;
            gameLogic.isPaused = true;
            if(classSelectMenuObj != null)
                classSelectMenuObj.SetActive(true);
        }
    }
}

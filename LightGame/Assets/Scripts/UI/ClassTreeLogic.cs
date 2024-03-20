using UnityEngine;
using UnityEngine.UI;

public class ClassTreeLogic : MonoBehaviour
{
    public GameObject class1SelectMenuObj;
    public GameObject class2SelectMenuObj;
    
    private GameLogic gameLogic;
    public bool isSelecting = false;

    // Class Objects
    public GameObject prefab_fighter;
    public GameObject prefab_ranger;
    public GameObject prefab_mage;

    void Start(){
        gameLogic = GameObject.FindWithTag("GameController").GetComponent<GameLogic>();
    }
    
    public void ClassSelect(string name){
        switch(name){
            case "Fighter": ReplacePlayer(prefab_fighter); break;
            case "Ranger": ReplacePlayer(prefab_ranger); break;
            case "Mage": ReplacePlayer(prefab_mage); break;
            default: Debug.LogError("Class Prefab ID out of range"); break;
        }
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
        ToggleClassSelectMenu(class1SelectMenuObj);
    }

    public void ToggleClassSelectMenu(GameObject menu){
        if(gameLogic == null) gameLogic = GameObject.FindWithTag("GameController").GetComponent<GameLogic>();

        if (gameLogic.isPaused){                    
            // Hide Class Selection Menu
            gameLogic.toggleCursor(false);
            isSelecting = false;

            Time.timeScale = 1f;
            gameLogic.isPaused = false;
            if(menu != null)
                menu.SetActive(false);
        }
        else{
            // Show Class Selection Menu
            gameLogic.toggleCursor(true);
            isSelecting = true;
            
            Time.timeScale = 0f;
            gameLogic.isPaused = true;
            if(menu != null)
                menu.SetActive(true);
        }
    }

    public void InvokeMenuClassSelect(int num){
        if(num == 1) ToggleClassSelectMenu(class1SelectMenuObj);
        else if(num == 2) ToggleClassSelectMenu(class2SelectMenuObj);
        else Debug.LogError("Invalid Class Menu Selection");
    }
}

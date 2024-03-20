using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ClassTreeLogic : MonoBehaviour
{
    public GameObject class1SelectMenuObj;
    public GameObject class2SelectMenuObj;
    
    private GameLogic gameLogic;
    public bool isSelecting = false;
    private int activeMenu = 0;

    // Class Objects
    public GameObject prefab_fighter;
    public GameObject prefab_ranger;
    public GameObject prefab_mage;
    // Fighter
    public GameObject prefab_knight;
    public GameObject prefab_berserker;
    // Ranger
    public GameObject prefab_sharpshooter;
    public GameObject prefab_rogue;
    // Mage
    public GameObject prefab_sorcerer;
    public GameObject prefab_wizard;

    void Start(){
        gameLogic = GameObject.FindWithTag("GameController").GetComponent<GameLogic>();
    }
    
    public void ClassSelect(string name){
        switch(name){
            case "Fighter": ReplacePlayer(prefab_fighter); break;
            case "Ranger": ReplacePlayer(prefab_ranger); break;
            case "Mage": ReplacePlayer(prefab_mage); break;
            case "Knight": ReplacePlayer(prefab_knight); break;
            case "Berserker": ReplacePlayer(prefab_berserker); break;
            case "Sharpshooter": ReplacePlayer(prefab_sharpshooter); break;
            case "Rogue": ReplacePlayer(prefab_rogue); break;
            case "Sorcerer": ReplacePlayer(prefab_sorcerer); break;
            case "Wizard": ReplacePlayer(prefab_wizard); break;
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
        ToggleClassSelectMenu(activeMenu == 1? class1SelectMenuObj : class2SelectMenuObj);
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
        activeMenu = num;
        if(num == 1) ToggleClassSelectMenu(class1SelectMenuObj);
        else if(num == 2){ 
            SetMenu2Options();
            ToggleClassSelectMenu(class2SelectMenuObj);
        }
        else Debug.LogError("Invalid Class Menu Selection");
    }

   private void SetMenu2Options(){
        if (gameLogic == null)
        {
            Debug.LogError("GameLogic is not assigned.");
            return;
        }

        string currClass = gameLogic.player.getClassName();
        Transform menuTransform = class2SelectMenuObj.transform;

        switch (currClass)
        {
            case "Fighter":
                Menu2ButtonSet(menuTransform, "ClassBtnA", "Berserker");
                Menu2ButtonSet(menuTransform, "ClassBtnB", "Knight");
                break;
            case "Ranger":
                Menu2ButtonSet(menuTransform, "ClassBtnA", "Sharpshooter");
                Menu2ButtonSet(menuTransform, "ClassBtnB", "Rogue");
                break;
            case "Mage":
                // Customize Mage menu options
                Menu2ButtonSet(menuTransform, "ClassBtnA", "Sorcerer");
                Menu2ButtonSet(menuTransform, "ClassBtnB", "Wizard");
                break;
            default:
                Debug.LogError("Invalid player class.");
                break;
        }
    }

    private void Menu2ButtonSet(Transform menuTransform, string btnName, string className)
    {
        TMP_Text buttonText = menuTransform.Find(btnName).GetComponentInChildren<TMP_Text>();
        buttonText.text = className;

        BindButtonAction(menuTransform.Find(btnName), buttonText.text, () => ClassSelect(className));
    }

    private void BindButtonAction(Transform buttonTransform, string buttonText, UnityEngine.Events.UnityAction action)
    {
        buttonTransform.GetComponent<Button>().onClick.RemoveAllListeners(); // Remove any previous listeners
        buttonTransform.GetComponent<Button>().onClick.AddListener(action);
    }

}

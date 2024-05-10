using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ClassTreeLogic : MonoBehaviour
{
    public GameObject class1SelectMenuObj;
    public GameObject class2SelectMenuObj;

    private int activeMenu = 0;

    // Class Objects
    public GameObject prefab_base;
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

    public void MenuClassSelect(string name)
    {
        ClassSelect(name);
    }

    public void ClassSelect(string name, bool toggleMenu = true)
    {
        switch (name)
        {
            case "Base": ReplacePlayer(prefab_base, toggleMenu); break;
            case "Fighter": ReplacePlayer(prefab_fighter, toggleMenu); break;
            case "Ranger": ReplacePlayer(prefab_ranger, toggleMenu); break;
            case "Mage": ReplacePlayer(prefab_mage, toggleMenu); break;
            case "Knight": ReplacePlayer(prefab_knight, toggleMenu); break;
            case "Berserker": ReplacePlayer(prefab_berserker, toggleMenu); break;
            case "Sharpshooter": ReplacePlayer(prefab_sharpshooter, toggleMenu); break;
            case "Rogue": ReplacePlayer(prefab_rogue, toggleMenu); break;
            case "Sorcerer": ReplacePlayer(prefab_sorcerer, toggleMenu); break;
            case "Wizard": ReplacePlayer(prefab_wizard, toggleMenu); break;
            default: Debug.LogError("Class Prefab ID out of range"); break;
        }
    }

    void ReplacePlayer(GameObject newPlayerPrefab, bool toggleMenu)
    {
        // Get old player info
        GameObject oldPlayer = GameObject.FindGameObjectWithTag("Player");
        oldPlayer.SetActive(false);

        Transform pivot = oldPlayer.transform.parent.transform.Find("CameraPivot").transform;
        Vector3 oldPlayerPosition = oldPlayer.transform.position;

        // Set old player rotation to have x-axis and z-axis rotation as 0
        Quaternion oldPlayerRotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);

        // Generate the new player
        GameObject newPlayer = Instantiate(newPlayerPrefab, oldPlayerPosition, oldPlayerRotation);

        // Remove old player
        Destroy(oldPlayer.transform.parent.gameObject);

        // Remove menu after choice is made
        if (toggleMenu)
            ToggleClassSelectMenu(activeMenu == 1 ? class1SelectMenuObj : class2SelectMenuObj, false);
    }

    public void ToggleClassSelectMenu(GameObject menu, bool show)
    {
        if (!show)
        {
            // Hide Class Selection Menu
            GameLogic.instance.toggleCursor(false);
            menu.SetActive(false);
            Debug.Log("HERE 1");
            Time.timeScale = 1;
        }
        else
        {
            // Show Class Selection Menu
            GameLogic.instance.toggleCursor(true);
            menu.SetActive(true);
            Debug.Log("HERE 2");
            Time.timeScale = 0;
        }
    }

    public void InvokeMenuClassSelect(int num)
    {
        activeMenu = num;
        if (num == 1) ToggleClassSelectMenu(class1SelectMenuObj, true);
        else if (num == 2)
        {
            SetMenu2Options();
            ToggleClassSelectMenu(class2SelectMenuObj, true);
        }
        else Debug.LogError("Invalid Class Menu Selection");
    }

    private void SetMenu2Options()
    {

        string currClass = GameLogic.instance.player.getClassName();
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

        BindButtonAction(menuTransform.Find(btnName), buttonText.text, () => MenuClassSelect(className));
    }

    private void BindButtonAction(Transform buttonTransform, string buttonText, UnityEngine.Events.UnityAction action)
    {
        buttonTransform.GetComponent<Button>().onClick.RemoveAllListeners(); // Remove any previous listeners
        buttonTransform.GetComponent<Button>().onClick.AddListener(action);
    }

}

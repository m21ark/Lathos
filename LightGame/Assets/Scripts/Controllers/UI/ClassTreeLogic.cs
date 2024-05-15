using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ClassTreeLogic : MonoBehaviour
{

    public static ClassTreeLogic instance { get; private set; }

    // Class Selection Menus
    [Header("Class Selection Menus")]
    public GameObject class1SelectMenuObj;
    public GameObject class2SelectMenuObj;

    private int activeMenu = 0;

    // Class Objects
    [Header("Class Prefabs")]
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

    // Completeness indicator
    [Header("Completeness Indicators")]
    public GameObject completenessA;
    public GameObject completenessB;
    public GameObject completenessC;

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("More than one ClassTreeLogic in the scene");
        else instance = this;
    }

    public void MenuClassSelect(string name)
    {
        ClassSelect(name);
    }

    public void OpenClassSelectionMenu()
    {
        string currClass = GameLogic.instance.player.getClassName();
        
        // First class 
        if (currClass == "Base"){

            // Check if any endings are unlocked and set completeness indicators
            bool[] ends = GameLogic.instance.endingsUnlocked;
            instance.completenessA.SetActive(ends[0]);
            instance.completenessB.SetActive(ends[1]);
            instance.completenessC.SetActive(ends[2]);

            instance.InvokeMenuClassSelect(1);
        }

        // Second class 
        List<string> classes1Names = new List<string> { "Fighter", "Ranger", "Mage" };

        // Check if the player's class name is in the list
        if (classes1Names.Contains(currClass))
            instance.InvokeMenuClassSelect(2);

        GameLogic.instance.RefreshPlayer();
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
            default: Debug.LogError("Class Prefab ID out of range: " + name); break;
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
            Time.timeScale = 1;
        }
        else
        {
            // Show Class Selection Menu
            GameLogic.instance.toggleCursor(true);
            menu.SetActive(true);
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

        TMP_Text description = menuTransform.Find(btnName + "Description").GetComponent<TMP_Text>();
        description.text = GetClassDescription(className);

        BindButtonAction(menuTransform.Find(btnName), buttonText.text, () => MenuClassSelect(className));
    }

    private void BindButtonAction(Transform buttonTransform, string buttonText, UnityEngine.Events.UnityAction action)
    {
        buttonTransform.GetComponent<Button>().onClick.RemoveAllListeners(); // Remove any previous listeners
        buttonTransform.GetComponent<Button>().onClick.AddListener(action);
    }

    private string GetClassDescription(string className){
        switch (className)
        {
            case "Berserker": return "Enraged battle-axe warrior with swift slashes. Lightning-fast dash assaults close the distance to their foes in an instant. When frenzy reaches its peak, strength is greatly amplified, unleashing unparalleled devastation";
            case "Knight": return "Longsword masters with powerful blows and signature moves, including a sweeping 360-degree rotating slash that strikes down all nearby foes and an ultimate Dimensional Slash technique that cuts through space itself to obliterate foes";
            case "Sharpshooter": return "Specialists in long-range warfare, possessing a keen sense of their surroundings, able to detect enemy presence with their Sound of Nature ability. Their special arrow technique pierces through enemies with devastating force";
            case "Rogue": return "Employ agile battle tactics by unleashing a barrage of thrown leaves, striking foes with surprising speed. Activating overdrive, they surge with increased attack speed, while their special move unleashes a devastating shotgun-like burst of leaves in a wide cone";
            case "Sorcerer": return "A cunning sorcery manipulator, casting magic that marks enemies for their impending doom, as accumulated marks can be triggered to originate light explosions. At the technique's zenith, waves of light can be evoked to mark all surrounding foes";
            case "Wizard": return "A master of light's essence that casts spells with imbued splash damage and can harness ancient knowledge to conjure massive energy balls. As an ultimate technique, divine beams can be summoned from the skies to incinerate any who dare cross their path";
        
        }
        return "Invalid class name";
    }
}

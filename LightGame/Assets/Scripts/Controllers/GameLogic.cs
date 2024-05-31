using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    public static GameLogic instance { get; private set;}

    // Entities
    [HideInInspector] public ProtoClass player;
    [HideInInspector] public Boss boss = null;
    public bool persistentData = false;

    public GameObject endMenu;

    [HideInInspector] public bool isInBossBattle;

    // Game Logic Fields
    private float lightDecreaseTimer = 0f;
    private int healTickTime = 5;
    [HideInInspector] public bool isPaused = false;
    [HideInInspector] public bool[] endingsUnlocked = { false, false, false };

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("More than one GameLogic in the scene");
        else instance = this;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        CheckIfInBossFight();
        RefreshPlayer();
        DataPersistentLoad();

        if (player == null)
        {
            Debug.LogError("GameObject with tag 'Player' was not found");
            Time.timeScale = 0;
            return;
        }

        // Set start time variables
        Time.timeScale = 1;

        toggleCursor(false); // Hide cursor during gameplay
    }

    public void CheckIfInBossFight(){
        GameObject bossObj = GameObject.FindWithTag("Boss");

        if (bossObj == null)
            isInBossBattle = false;
        else
        {
            isInBossBattle = true;
            boss = bossObj.GetComponent<Boss>();
        }
    }

    public void RefreshPlayer()
    {
        player = GameObject.FindWithTag("Player").GetComponent<ProtoClass>();
    }

    // Update is called once per frame
    void Update()
    {
        RefreshPlayer();

        handlePlayerLight();

        // TODO: For debugging purposes
        if (Input.GetKeyDown(KeyCode.H)){
            player.Heal(100);
            player.IncrementLight(100);
        }
            
        // Check end game conditions
        if (isInBossBattle)
            if (boss.health <= 0) StartCoroutine(endGame(true));
        if (!player.isAlive()) StartCoroutine(endGame(false));
    }

    void handlePlayerLight()
    {

        lightDecreaseTimer += Time.deltaTime;

        // If one second has passed, decrease the player's light or damage them if they have no light
        if (lightDecreaseTimer >= 1f)
        {
            lightDecreaseTimer -= 1f;
            healTickTime--;

            if (player.collectedLight > 0){
                player.IncrementLight(-1);

                // If player life is below max, heal them expending light points (once every 5 seconds)
                if (!player.IsAtMaxHealth() && healTickTime <=0)
                {
                    player.Heal(10);
                    player.IncrementLight(-5);
                    healTickTime = 5;
                }
            } else player.TakeDamage(1);
        }
    }

    public void DataPersistentSave()
    {
        if (!persistentData) return;

        // Build the data to save
        SaveData data = new SaveData();
        data.currentPlayerArea = ((int)SceneManager.GetActiveScene().buildIndex + 1);
        data.playerClassName = player.getClassName();
        data.unlockedEndings = this.endingsUnlocked;

        // Save the data
        SaveSystem.DataSave(data);
    }

    void DataPersistSaveNewEnding(){
        if (!persistentData) return;

        SaveData data = new SaveData();
        data.currentPlayerArea = 1;
        data.playerClassName = "Base";
        data.unlockedEndings = this.endingsUnlocked;

        SaveSystem.DataSave(data);
    }

    void DataPersistentLoad()
    {
        if (!persistentData) return;

        SaveData data = SaveSystem.DataLoad();

        // if there is no available data, use the default values
        if (data == null)
        {
            Debug.Log("No player data found. Starting new game save.");
            data = new SaveData();
        }

        // go to the scene
        if (data.currentPlayerArea != SceneManager.GetActiveScene().buildIndex)
        {
            Debug.Log("Loading saved scene: " + data.currentPlayerArea);
            SceneManager.LoadScene(data.currentPlayerArea);
        }
        else
        {
            // Put the data in the game
            this.endingsUnlocked = data.unlockedEndings;

            if (data.playerClassName != player.getClassName())
                ClassTreeLogic.instance.ClassSelect(data.playerClassName, false);
        }
    }

    IEnumerator endGame(bool playerWon)
    {
        if(!playerWon){
            TextMeshProUGUI end_msg = endMenu.transform.Find("EndGameSMS").GetComponent<TextMeshProUGUI>();
            end_msg.text = "You died!";
            GameObject PlayAgainBtn = endMenu.transform.Find("PlayAgainBtn").gameObject;
            PlayAgainBtn.SetActive(true);
            endMenu.SetActive(true);
            toggleCursor(true);
        }else{

            player.Heal(100);
            player.IncrementLight(100);
        
            string playerClass = player.getClassName();

            switch (playerClass){
                case "Berserker":
                case "Knight":
                    endingsUnlocked[0] = true;
                    AudioManager.instance.PlayEnding(0);
                    DialogueController.instance.StartDialogue("Fighter Ending");
                    break;
                case "Sharpshooter":
                case "Rogue":
                    endingsUnlocked[1] = true;
                    AudioManager.instance.PlayEnding(1);
                    DialogueController.instance.StartDialogue("Ranger Ending");
                    break;
                case "Sorcerer":
                case "Wizard":
                    endingsUnlocked[2] = true;
                    AudioManager.instance.PlayEnding(2);
                    DialogueController.instance.StartDialogue("Mage Ending");
                    break;
                default:
                    Debug.LogError("Tried to save player class that isn't a valid subclass: " + playerClass);
                    break;
            }

            DataPersistSaveNewEnding();

            while(!DialogueController.instance.isDialogueOver)
                yield return new WaitForSecondsRealtime(1); 

            int endingsCount = 0;
            foreach (bool ending in endingsUnlocked)
                if (ending) endingsCount++;
            
            TextMeshProUGUI end_msg = endMenu.transform.Find("EndGameSMS").GetComponent<TextMeshProUGUI>();
            end_msg.text = string.Format("You won!\n\n({0}/3 endings unlocked)", endingsCount);
            GameObject PlayAgainBtn = endMenu.transform.Find("PlayAgainBtn").gameObject;

            PlayAgainBtn.SetActive(false);
            endMenu.SetActive(true);
            toggleCursor(true);
        }
    }

    public void ResetScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void toggleCursor(bool show)
    {
        // Lock mouse and make it invisible during gameplay
        // And show it again in menus for selection
        if (show)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            isPaused = true;
            Time.timeScale = 0;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            isPaused = false;
            Time.timeScale = 1;
        }
    }

}

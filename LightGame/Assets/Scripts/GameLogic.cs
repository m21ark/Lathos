using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{

    // Entities
    [HideInInspector] public ProtoClass player;
    [HideInInspector] public Boss boss;

    public GameObject endMenu;
    public DialogueController dialogueController;
    public DialogueController fullScreenDialogueController;

    private ClassTreeLogic classTreeLogic;
    private bool isInBossBattle;

    // Game Logic Fields
    private float gameTime = 0.0f;
    private float lightDecreaseTimer = 0f;
    private bool isShowingFullScreenDialogue = false;
    [HideInInspector] public bool isPaused = false;
    private bool[] endingsUnlocked = { false, false, false };

    // Dialogue Data
    public List<DialogueData> dialogueDataList;

    // HUD Text Elements
    private GameObject hud;
    private TextMeshProUGUI hud_timer;
    private TextMeshProUGUI hud_player_health;
    private TextMeshProUGUI hud_boss_health;
    private TextMeshProUGUI hud_light;
    private TextMeshProUGUI hud_playerClassName;
    private TextMeshProUGUI hud_playerCooldowns;

    // Start is called before the first frame update
    void Start()
    {

        GameObject bossObj = GameObject.FindWithTag("Boss");
        classTreeLogic = gameObject.GetComponent<ClassTreeLogic>();
        
        if(bossObj == null){
            isInBossBattle = false;
        }else{
            isInBossBattle = true;
            boss = bossObj.GetComponent<Boss>();
        }

        HUDLoadElements();
        RefreshPlayer();
        // DataPersistentLoad(); // TODO: Uncomment this line to enable load functionality
        
        if (player == null){
            Debug.LogError("GameObject with tag 'Player' was not found");
            Time.timeScale = 0;
            return;
        }
        
        // Set start time variables
        Time.timeScale = 1;
        gameTime = 0.0f;  

        toggleCursor(false); // Hide cursor during gameplay
    }

    public void RefreshPlayer(){
        player = GameObject.FindWithTag("Player").GetComponent<ProtoClass>();
    }

    void HUDLoadElements(){
        hud = GameObject.FindWithTag("HUD");
        if(hud){
            hud_timer = hud.transform.Find("Timer").GetComponent<TextMeshProUGUI>();
            hud_player_health = hud.transform.Find("PlayerHealth").GetComponent<TextMeshProUGUI>();
            hud_boss_health = hud.transform.Find("BossHealth").GetComponent<TextMeshProUGUI>();
            hud_light = hud.transform.Find("LightCounter").GetComponent<TextMeshProUGUI>();
            hud_playerClassName = hud.transform.Find("CurrentPlayerClass").GetComponent<TextMeshProUGUI>();
            hud_playerCooldowns = hud.transform.Find("PlayerCooldowns").GetComponent<TextMeshProUGUI>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        RefreshPlayer();

        handlePlayerLight();

        // Update the game time
        gameTime += Time.deltaTime;

        // Update the HUD
        if(hud != null) updateHUD();

        // TODO: For debugging purposes
        if (Input.GetKeyDown(KeyCode.H))
            player.Heal(100);

        // If boss or player dead, end game
        if(isInBossBattle){
            if(boss.health <= 0) endGame(true);
        }
        if(!player.isAlive()) endGame(false);

        if(classTreeLogic == null)
            classTreeLogic = gameObject.GetComponent<ClassTreeLogic>();

        if(!classTreeLogic.isSelecting && Input.GetKeyDown(KeyCode.C))
            checkClassSelectionTrigger();

        CheckDialogue();
    }

    void handlePlayerLight() {

        lightDecreaseTimer += Time.deltaTime;
        // If one second has passed, decrease the player's light
        if (lightDecreaseTimer >= 1f) {
            lightDecreaseTimer -= 1f;
            if (player.collectedLight > 0) {
                player.collectedLight -= 1;
                if (player.collectedLight < 0) player.collectedLight = 0;
            }

            // If player's light is 0, give him damage
            if (player.collectedLight == 0) {
                player.TakeDamage(1);
            }
        }
    }

    public void StartDialogue(string key, bool isFullScreen = false)
    {
        if(isFullScreen && isShowingFullScreenDialogue) return;

        DialogueData data = dialogueDataList.Find(data => data.dialogueName == key);
        if (data != null){
            if(isFullScreen){
                    toggleCursor(true);
                    fullScreenDialogueController.Display(data);
                    isShowingFullScreenDialogue = true;
            } else
                dialogueController.Display(data);
        }
        else Debug.LogError("Dialogue data with key '" + key + "' not found.");
    }

    void CheckDialogue(){
        // Check if the full screen dialogue is done to unpause the game 
        if(isShowingFullScreenDialogue && !fullScreenDialogueController.isActive){
            isShowingFullScreenDialogue = false;
            toggleCursor(false);
        }
    }

    public void DataPersistentSave(){
        // Build the data to save
        SaveData data = new SaveData();
        data.currentPlayerArea = ((int)SceneManager.GetActiveScene().buildIndex + 1);
        data.playerClassName = player.getClassName();
        data.unlockedEndings = this.endingsUnlocked;

        // Save the data
        SaveSystem.DataSave(data);
    }

    void DataPersistentLoad(){
        SaveData data = SaveSystem.DataLoad();

        // if there is no available data, use the default values
        if(data == null){
            Debug.Log("No player data found. Starting new game save.");
            data = new SaveData();
        }

        // go to the scene
        if(data.currentPlayerArea != SceneManager.GetActiveScene().buildIndex){
            Debug.Log("Loading saved scene: " + data.currentPlayerArea);
            SceneManager.LoadScene(data.currentPlayerArea);
        }else{
            // Put the data in the game
            this.endingsUnlocked = data.unlockedEndings;
            
            if(data.playerClassName != player.getClassName())
                classTreeLogic.ClassSelect(data.playerClassName, false);
        }
    }

    void checkClassSelectionTrigger(){

        // First class 
        if(player.getClassName() == "Base"){ 
            Debug.Log("Trigger Class 1 Selection");
            classTreeLogic.InvokeMenuClassSelect(1);
        }

        // Second class 
        List<string> classes1Names = new List<string> { "Fighter", "Ranger", "Mage" };

        // Check if the player's class name is in the list
        if (classes1Names.Contains(player.getClassName())){
            Debug.Log("Trigger Class 2 Selection");
            classTreeLogic.InvokeMenuClassSelect(2);
        }

        RefreshPlayer();
    }

    void endGame(bool playerWon){
        TextMeshProUGUI end_msg = endMenu.transform.Find("EndGameSMS").GetComponent<TextMeshProUGUI>();
        end_msg.text = string.Format("{0}", playerWon? "You won!" : "You died!" );
        endMenu.SetActive(true);
        toggleCursor(true);
    }

    public void ResetScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void updateHUD(){

        // Update timer in the format MM:SS
        int minutes = Mathf.FloorToInt(gameTime/ 60F);
        int seconds = Mathf.FloorToInt(gameTime - minutes * 60);
        hud_timer.text = string.Format("Time: {0:0}:{1:00}", minutes, seconds);

        // Update boss and player's health 
        hud_player_health.text = string.Format("Health: {0}", player.health > 0 ? player.health : 0);
        if(isInBossBattle)
            hud_boss_health.text = string.Format("Boss Health: {0}", boss.health > 0 ? boss.health : 0);
        else hud_boss_health.text = "";

        // Update light collected and current player class
        hud_light.text = string.Format("Light: {0}", player.collectedLight);
        hud_playerClassName.text = string.Format("Class: {0}", player.getClassName());

        // Update player's cooldowns
        string baseCool = CooldownFormat(player.lastAttackTime);
        string classCool = CooldownFormat(player.lastAttack1Time);
        string AbilityCool = CooldownFormat(player.lastAttack2Time);
        string DashCool = CooldownFormat(player.lastDashTime);
        hud_playerCooldowns.text = string.Format("Base: {0}\nClass: {1}\nAbility: {2}\nDash: {3}", baseCool, classCool, AbilityCool, DashCool);
        
    }

    private string CooldownFormat(float playerCooldownTimer){
        return string.Format("{0}",  playerCooldownTimer <= 0 ? "Ready" : (Mathf.Round(playerCooldownTimer * 100f) / 100f));
    }

    public void toggleCursor(bool show){
        // Lock mouse and make it invisible during gameplay
        // And show it again in menus for selection
        if(show){
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            isPaused = true;
            Time.timeScale = 0;
        }else{
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            isPaused = false;
            Time.timeScale = 1;
        }
    }

}

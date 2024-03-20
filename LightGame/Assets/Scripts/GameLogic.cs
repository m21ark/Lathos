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

    private ClassTreeLogic classTreeLogic;

    // Costs to rank up class
    public int class1Cost = 5;
    public int class2Cost = 10;

    // Game Logic Fields
    private float gameTime = 0.0f;
    public bool isPaused = false;

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
        HUDLoadElements();

        RefreshPlayer();
        boss = GameObject.FindWithTag("Boss").GetComponent<Boss>();

        classTreeLogic = gameObject.GetComponent<ClassTreeLogic>();

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

    private void RefreshPlayer(){
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

        // Update the game time
        gameTime += Time.deltaTime;

        // Update the HUD
        if(hud != null) updateHUD();

        // For debugging purposes
        if (Input.GetKeyDown(KeyCode.H))
            player.Heal(20);

        // If boss or player dead, end game
        if(boss != null && boss.health <= 0)
            endGame(true);
        else if(!player.isAlive())
            endGame(false);

        if(!classTreeLogic.isSelecting)
            checkClassSelectionTrigger();
    }

    void checkClassSelectionTrigger(){

        // First class 
        if(player.collectedLight >= class1Cost && player.getClassName() == "Base"){ 
            Debug.Log("Trigger Class 1 Selection");
            classTreeLogic.ToggleClassSelectMenu();
            class1Cost = int.MaxValue; // This line is necessary
        }

        // Second class 
        List<string> classes1Names = new List<string> { "Fighter", "Ranger", "Mage" };

        // Check if the player's class name is in the list
        if (player.collectedLight >= class2Cost && classes1Names.Contains(player.getClassName())){
            Debug.Log("Trigger Class 2 Selection");
            class2Cost = int.MaxValue; // This line is necessary
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
        hud_boss_health.text = string.Format("Boss Health: {0}", boss.health > 0 ? boss.health : 0);

        // Update light collected and current player class
        hud_light.text = string.Format("Light: {0}", player.collectedLight);
        hud_playerClassName.text = string.Format("Class: {0}", player.getClassName());

        // Update player's cooldowns
        string baseCool = CooldownFormat(player.lastAttackTime);
        string classCool = CooldownFormat(player.lastBaseAttackTime);
        string AbilityCool = CooldownFormat(player.lastAbilityAttackTime);
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

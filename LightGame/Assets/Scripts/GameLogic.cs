using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{

    // Entities
    public Player player;
    public GameObject endMenu;

    public int bossHealth = 300;

    // Game Logic Fields
    private float gameTime = 0.0f;
    public bool isPaused = false;

    // HUD Text Elements
    private GameObject hud;
    private TextMeshProUGUI hud_timer;
    private TextMeshProUGUI hud_player_health;
    private TextMeshProUGUI hud_boss_health;
    private TextMeshProUGUI hud_light;

    // Start is called before the first frame update
    void Start()
    {
        HUDLoadElements();

        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (player == null){
            Debug.Log("PLAYER NOT FOUND");
            Time.timeScale = 0;
            return;
        }
        
        // Set start time variables
        Time.timeScale = 1;
        gameTime = 0.0f;  

        toggleCursor(false); // Hide cursor during gameplay
    }

    void HUDLoadElements(){
        hud = GameObject.FindWithTag("HUD");
        if(hud){
            hud_timer = hud.transform.Find("Timer").GetComponent<TextMeshProUGUI>();
            hud_player_health = hud.transform.Find("PlayerHealth").GetComponent<TextMeshProUGUI>();
            hud_boss_health = hud.transform.Find("BossHealth").GetComponent<TextMeshProUGUI>();
            hud_light = hud.transform.Find("LightCounter").GetComponent<TextMeshProUGUI>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update the game time
        gameTime += Time.deltaTime;

        // Update the HUD
        if(hud != null) updateHUD();

        // If R is pressed, reset scene
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetScene();
        }

        // If boss or player dead, end game
        if(bossHealth <= 0)
            endGame(true);
        else if(!player.isAlive())
            endGame(false);
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
        hud_boss_health.text = string.Format("Boss Health: {0}", bossHealth > 0 ? bossHealth : 0);
        hud_light.text = string.Format("Light: {0}", player.light);
    }

    public void damageBoss(int damage){
        bossHealth -= damage;   
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

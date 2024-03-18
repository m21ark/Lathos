using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{

    public GameObject player;
    private GameObject hud;

    public int playerHealth = 100;
    public int bossHealth = 300;

    private float gameTime = 0.0f;

    public bool isPaused = false;


    // HUD Text Elements
    TextMeshProUGUI hud_timer;
    TextMeshProUGUI hud_player_health;
    TextMeshProUGUI hud_boss_health;
    TextMeshProUGUI hud_end_msg;

    // Start is called before the first frame update
    void Start()
    {
        HUDLoadElements();

        player = GameObject.FindWithTag("Player");
        if(player == null) Debug.Log("Couldn't find player object...");
        
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
            hud_end_msg = hud.transform.Find("EndGameSMS").GetComponent<TextMeshProUGUI>();
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
        else if(playerHealth <= 0)
            endGame(false);
    }

    void endGame(bool playerWon){
        hud_end_msg.text = string.Format("{0}", playerWon? "You won!" : "You died!" );
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
        hud_player_health.text = string.Format("Health: {0}", playerHealth > 0 ? playerHealth : 0);
        hud_boss_health.text = string.Format("Boss Health: {0}", bossHealth > 0 ? bossHealth : 0);
    }

    public void damageBoss(int damage){
        bossHealth -= damage;   
    }

    public void damagePlayer(int damage){
        playerHealth -= damage;   
    }

    public void toggleCursor(bool show){
        // Lock mouse and make it invisible during gameplay
        // And show it again in menus for selection
        if(show){
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            isPaused = true;
        }else{
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            isPaused = false;
        }
    }

}

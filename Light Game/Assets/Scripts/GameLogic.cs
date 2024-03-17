using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{

    public GameObject player;
    public GameObject hud;

    private int playerHealth = 100;
    private int bossHealth = 300;

    private float gameTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        hud = GameObject.FindWithTag("HUD");
        gameTime = 0.0f;   
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
    }

    public void ResetScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void updateHUD(){

        // Update timer in the format MM:SS
        TextMeshProUGUI hud_timer = hud.transform.Find("Timer").GetComponent<TextMeshProUGUI>();
        int minutes = Mathf.FloorToInt(gameTime/ 60F);
        int seconds = Mathf.FloorToInt(gameTime - minutes * 60);
        hud_timer.text = string.Format("Time: {0:0}:{1:00}", minutes, seconds);

        // Update boss and player's health 
        TextMeshProUGUI hud_player_health = hud.transform.Find("PlayerHealth").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI hud_boss_health = hud.transform.Find("BossHealth").GetComponent<TextMeshProUGUI>();

        hud_player_health.text = string.Format("Health: {0}", playerHealth);
        hud_boss_health.text = string.Format("Boss Health: {0}", bossHealth);
    }

    public void damageBoss(int damage){
        bossHealth -= damage;   
    }

    public void damagePlayer(int damage){
        playerHealth -= damage;   
    }

}

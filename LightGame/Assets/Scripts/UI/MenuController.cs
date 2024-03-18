using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject pauseMenuObj;
    public GameObject instructionsMenuObj;

    private GameLogic gameLogic;

    void Start(){
        try{
            gameLogic = GameObject.FindWithTag("GameController").GetComponent<GameLogic>();
        }
        catch (Exception e){
            Debug.Log(e);
        }
    }

    void Update(){
         // Check if ESC is pressed to pause Game
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePauseGame();
    }
    
    public void StartGame()
    {
        Debug.Log("Loading Boss Arena");
        SceneManager.LoadScene("BossArena");
    }

    public void ResumeGame(){
        TogglePauseGame();
    }

    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        gameLogic.ResetScene();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void GoToStartMenu()
    {
        Debug.Log("Going to start menu...");
        SceneManager.LoadScene("StartMenu");
    }


    public void TogglePauseGame()
    {
        if(gameLogic == null) gameLogic = GameObject.FindWithTag("GameController").GetComponent<GameLogic>();

        if (gameLogic.isPaused){
            // Resume Game
                    
            // Disable mouse cursor again for gameplay
            gameLogic.toggleCursor(false);

            Time.timeScale = 1f;
            gameLogic.isPaused = false;
            if(pauseMenuObj != null)
                pauseMenuObj.SetActive(false);
        }
        else{
        
            // Pause Game
            
            // Re enable mouse cursor for the pause menu
            gameLogic.toggleCursor(true);
            
            Time.timeScale = 0f;
            gameLogic.isPaused = true;
            if(pauseMenuObj != null)
                pauseMenuObj.SetActive(true);
        }
    }

}

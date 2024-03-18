using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public static bool isGamePaused = false;

    public GameObject pauseMenuObj;
    private GameLogic gameLogic;


    void Start(){
        gameLogic = GameObject.FindWithTag("GameController").GetComponent<GameLogic>();
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
        if(isGamePaused) ResumeGame();
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

    public void ResumeGame()
    {
        // Disable mouse cursor again for gameplay
        gameLogic.toggleCursor(false);

        Time.timeScale = 1f;
        isGamePaused = false;
        if(pauseMenuObj != null)
            pauseMenuObj.SetActive(false);
    }

    public void PauseGame()
    {
        // Re enable mouse cursor for the pause menu
        gameLogic.toggleCursor(true);
        
        Time.timeScale = 0f;
        isGamePaused = true;
        if(pauseMenuObj != null)
            pauseMenuObj.SetActive(true);
    }

    public void TogglePauseGame()
    {
        if (isGamePaused) ResumeGame();
        else PauseGame();
    }

}

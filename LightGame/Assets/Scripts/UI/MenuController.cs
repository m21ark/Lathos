using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject pauseMenuObj;
    public GameObject instructionsMenuObj;

    private GameLogic gameLogic;

    void Start(){
        GameObject gameController = GameObject.FindWithTag("GameController");
        if(gameController != null)
            gameLogic = gameController.GetComponent<GameLogic>();
        
        LoadPlayerSettings();
    }

    void Update(){
         // Check if ESC is pressed to pause Game
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePauseGame();
    }

    public void SavePlayerSettings(){
        Debug.Log("Saving new player settings...");
        GameObject settingsMenu = gameObject.transform.Find("SettingsMenu").gameObject;

        // Get the 2 sliders
        Slider sfxVolumeSlider = settingsMenu.transform.Find("SoundSlider").GetComponent<Slider>();
        Slider musicVolumeSlider = settingsMenu.transform.Find("MusicSlider").GetComponent<Slider>();

        // Save player preferences for the sliders
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
        PlayerPrefs.Save();
    }

    public void LoadPlayerSettings(){
        GameObject settingsMenu = gameObject.transform.Find("SettingsMenu").gameObject;

        // Get the 2 sliders
        Slider sfxVolumeSlider = settingsMenu.transform.Find("SoundSlider").GetComponent<Slider>();
        Slider musicVolumeSlider = settingsMenu.transform.Find("MusicSlider").GetComponent<Slider>();

        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
    }

    public void GotoPlayMenu(){

        GameObject playNewMenu = gameObject.transform.Find("PlayNewMenu").gameObject;
        GameObject loadGameMenu = gameObject.transform.Find("PlayLoadMenu").gameObject;

        if(SaveSystem.SaveExists()){
            playNewMenu.SetActive(false);
            loadGameMenu.SetActive(true);
        }else{
            playNewMenu.SetActive(true);
            loadGameMenu.SetActive(false);
        }
    }

    public void DeleteGameInstance(){
        Debug.Log("Deleting game instance...");
        SaveSystem.DeleteSave();
    }
    
    public void StartNewGame()
    {
        Debug.Log("Starting new game...");
        SceneManager.LoadScene("Room1");
    }

    public void StartLoadGame()
    {
        Debug.Log("Loading stored game instance...");
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

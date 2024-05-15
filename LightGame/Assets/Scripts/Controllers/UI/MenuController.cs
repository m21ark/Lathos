using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuController : MonoBehaviour
{
    public GameObject pauseMenuObj;
    private SaveData data = null;

    public TMP_Text gameStateText;

    // Completeness indicator
    [Header("Completeness Indicators")]
    public GameObject completenessA;
    public GameObject completenessB;
    public GameObject completenessC;

    void Start()
    {
        LoadPlayerSettings();
    }

    void Update()
    {
        // Check if ESC is pressed to pause Game
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePauseGame();
    }

    void SetUnlockedEndingsIndicators()
    {
        // Check if any endings are unlocked and set completeness indicators
        completenessA.SetActive(data.unlockedEndings[0]);
        completenessB.SetActive(data.unlockedEndings[1]);
        completenessC.SetActive(data.unlockedEndings[2]);
    }

    public void SavePlayerSettings()
    {
        Transform settingsMenu = gameObject.transform.Find("SettingsMenu").gameObject.transform;

        // Get the 4 sliders
        Slider master = settingsMenu.Find("SliderMaster").GetComponent<Slider>();
        Slider sfx = settingsMenu.Find("SliderSFX").GetComponent<Slider>();
        Slider music = settingsMenu.Find("SliderMusic").GetComponent<Slider>();
        Slider ambience = settingsMenu.Find("SliderAmbience").GetComponent<Slider>();

        // Save player preferences for the sliders
        PlayerPrefs.SetFloat("MasterVolume", master.value);
        PlayerPrefs.SetFloat("SFXVolume", sfx.value);
        PlayerPrefs.SetFloat("MusicVolume", music.value);
        PlayerPrefs.SetFloat("AmbienceVolume", ambience.value);
        PlayerPrefs.Save();
    }

    public void LoadPlayerSettings()
    {
        try
        {
            Transform settingsMenu = gameObject.transform.Find("SettingsMenu").gameObject.transform;

            // Get the 4 sliders
            Slider master = settingsMenu.Find("SliderMaster").GetComponent<Slider>();
            Slider sfx = settingsMenu.Find("SliderSFX").GetComponent<Slider>();
            Slider music = settingsMenu.Find("SliderMusic").GetComponent<Slider>();
            Slider ambience = settingsMenu.Find("SliderAmbience").GetComponent<Slider>();

            // Load player preferences for the sliders
            master.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
            sfx.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
            music.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
            ambience.value = PlayerPrefs.GetFloat("AmbienceVolume", 1f);

            // Load game state if exists
            if(SaveSystem.SaveExists()){
                data = SaveSystem.DataLoad();
                SetUnlockedEndingsIndicators();
            }   
        }
        catch
        {
            // pass
        }
    }

    public void GotoPlayMenu()
    {

        GameObject playNewMenu = gameObject.transform.Find("PlayNewMenu").gameObject;
        GameObject loadGameMenu = gameObject.transform.Find("PlayLoadMenu").gameObject;

        if (SaveSystem.SaveExists())
        {
            playNewMenu.SetActive(false);
            loadGameMenu.SetActive(true);

            // Set gameStateText
            string txt;
            if(data.currentPlayerArea != 1){
                txt = "You have an instance of the game: \n";

                if(data.currentPlayerArea == 6) txt += "Area: Boss Fight\n";
                else  txt += "Area: " + data.currentPlayerArea + "\n";
               
                txt += "Class: " + data.playerClassName + "\n";  

            }else  txt = "Restart your journey with other of the 3 starter classes to unlock secret endings!";

            if(data.unlockedEndings[0] && data.unlockedEndings[1] && data.unlockedEndings[2])
                txt = "Congratulations for unlocking all endings!\n Thanks for playing!";
            

            gameStateText.text = txt;
            
        }
        else
        {
            playNewMenu.SetActive(true);
            loadGameMenu.SetActive(false);
        }
    }

    public void DeleteGameInstance()
    {
        Debug.Log("Deleting game instance...");
        SaveSystem.DeleteSave();
        data = null;
        completenessA.SetActive(false);
        completenessB.SetActive(false);
        completenessC.SetActive(false);
    }

    public void StartNewGame()
    {
        Debug.Log("Starting new game...");
        SceneManager.LoadScene("Room1");
    }

    public void StartLoadGame()
    {
        Debug.Log("Loading stored game instance...");

        // go to the scene
        if (data.currentPlayerArea != SceneManager.GetActiveScene().buildIndex)
            SceneManager.LoadScene(data.currentPlayerArea);
    }

    public void ResumeGame()
    {
        TogglePauseGame();
    }

    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        GameLogic.instance.ResetScene();
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

        if (GameLogic.instance.isPaused)
        {
            // Resume Game

            // Disable mouse cursor again for gameplay
            GameLogic.instance.toggleCursor(false);

            Time.timeScale = 1f;
            GameLogic.instance.isPaused = false;
            if (pauseMenuObj != null)
                pauseMenuObj.SetActive(false);
        }
        else
        {

            // Pause Game

            // Re enable mouse cursor for the pause menu
            GameLogic.instance.toggleCursor(true);

            Time.timeScale = 0f;
            GameLogic.instance.isPaused = true;
            if (pauseMenuObj != null)
                pauseMenuObj.SetActive(true);
        }
    }

}

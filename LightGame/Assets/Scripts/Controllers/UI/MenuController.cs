using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject pauseMenuObj;

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
    }

    public void StartNewGame()
    {
        Debug.Log("Starting new game...");
        SceneManager.LoadScene("Room1");
    }

    public void StartLoadGame()
    {
        Debug.Log("Loading stored game instance...");

        // Get the data
        SaveData data = SaveSystem.DataLoad();

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

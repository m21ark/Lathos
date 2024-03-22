using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTeleporter : MonoBehaviour
{
    private GameObject loadingScreen;
    private GameObject gameLogic;

    public float teleportDelay = 3f;
    public string areaName = ""; 

    void Start(){
        
        gameLogic = GameObject.FindWithTag("GameController");
        GameObject ui = gameLogic.transform.parent.transform.Find("UI").gameObject;
        loadingScreen = ui.transform.Find("LoadMenu").gameObject;

        if(loadingScreen == null)
            Debug.LogError("Loading screen not found!");
    }

    void OnTriggerEnter(Collider other)
    {
        if(areaName == ""){
            Debug.LogError("Area name not set for teleporter!");
            return;
        }

        if (other.CompareTag("Player"))
            StartCoroutine(GotoArea());
    }

    IEnumerator GotoArea(){
        loadingScreen.SetActive(true);
        Time.timeScale = 0; // Pause the game
        yield return new WaitForSecondsRealtime(teleportDelay);
        loadingScreen.SetActive(false);
        SceneManager.LoadScene(areaName);
        Time.timeScale = 1; // Resume the game after loading is complete
    }
}

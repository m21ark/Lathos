using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTeleporter : MonoBehaviour
{
    private GameObject loadingScreen;
    private GameObject gameLogic;
    private CanvasGroup fadeCanvasGroup;
    private CanvasGroup hudCanvasGroup;

    public float fadeDuration = 0.5f;
    public float teleportDelay = 3f;
    public string areaName = "";

    void Start()
    {
        gameLogic = GameObject.FindWithTag("GameController");
        GameObject ui = gameLogic.transform.parent.transform.Find("UI").gameObject;
        loadingScreen = ui.transform.Find("LoadMenu").gameObject;
        fadeCanvasGroup = ui.transform.Find("FadeCanvas").GetComponent<CanvasGroup>();
        hudCanvasGroup = ui.transform.Find("HUD").GetComponent<CanvasGroup>();
        
        if (loadingScreen == null || fadeCanvasGroup == null)
            Debug.LogError("Loading screen or fade canvas not found!");
    }

    void OnTriggerEnter(Collider other)
    {
        if (areaName == "")
        {
            Debug.LogError("Area name not set for teleporter!");
            return;
        }

        if (other.CompareTag("Player"))
            StartCoroutine(GotoArea());
    }

    IEnumerator GotoArea()
    {
        // TODO: Uncomment this line to enable load functionality
        // gameLogic.GetComponent<GameLogic>().DataPersistentSave();

        // Pause the game
        Time.timeScale = 0;

        // Fade to black
        float timer = 0f;
        while (timer < fadeDuration)
        {
            hudCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            timer += Time.unscaledDeltaTime; 
            yield return null;
        }
        fadeCanvasGroup.alpha = 1f;
        hudCanvasGroup.alpha = 0f;

        // Activate loading screen
        loadingScreen.SetActive(true);

        // Wait for a few seconds
        yield return new WaitForSecondsRealtime(teleportDelay);

        timer = 0f;
        while (timer < fadeDuration)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            timer += Time.unscaledDeltaTime; 
            yield return null;
        }
        fadeCanvasGroup.alpha = 1f;
        hudCanvasGroup.alpha = 1f;

        // Load the new scene
        SceneManager.LoadScene(areaName);
    }
}

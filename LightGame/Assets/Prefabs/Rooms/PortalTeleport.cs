using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTeleport : MonoBehaviour
{
    public Transform destination; // The destination where the player should be teleported
    public GameObject loadingScreen; // Reference to the loading screen canvas or UI element
    public float teleportDelay = 3f; // Delay before teleportation in seconds
    public bool loadBossArena = false;

    // Check for collision with the trigger
    void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has the "Player" tag
        if (other.CompareTag("Player"))
        {
            if(loadBossArena) StartCoroutine(GotoBossArena(other.gameObject));
            else StartCoroutine(TeleportPlayer(other.gameObject));
        }
    }

    IEnumerator GotoBossArena(GameObject player){
        // Activate the loading screen
        loadingScreen.SetActive(true);

        // Wait for the delay
        yield return new WaitForSeconds(teleportDelay);

        // Deactivate the loading screen
        loadingScreen.SetActive(false);

        Debug.Log("Loading Boss Arena");
        SceneManager.LoadScene("BossArena");
    }

    // Coroutine for teleporting the player
    IEnumerator TeleportPlayer(GameObject player)
    {
        // Activate the loading screen
        loadingScreen.SetActive(true);

        // Wait for the delay
        yield return new WaitForSeconds(teleportDelay);

        // Teleport the player to the destination
        player.transform.position = destination.position;

        // Deactivate the loading screen
        loadingScreen.SetActive(false);
    }
}

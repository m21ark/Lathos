using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightOrbBehavior : MonoBehaviour
{
    public GameObject lamp; // Reference to the orb GameObject
    public float accelerationSpeed = 40f; // Speed at which the object accelerates towards the orb

    public float maxDistance = 40f; // Distance at which the object starts to accelerate towards the orb

    public float baseSpeed = 0.5f; // Base speed at which the object moves

    private GameLogic gameLogic; // Reference to the GameLogic script

    private void Start()
    {
        // Find the lamp object in the scene
        lamp = GameObject.FindWithTag("Lamp");
        gameLogic = GameObject.FindWithTag("GameController").GetComponent<GameLogic>();
    }

    private void Update()
    {
        lamp = GameObject.FindWithTag("Lamp");

        // Check if the orb object is set
        if (lamp != null)
        {
            // Calculate the distance between this object and the orb
            float distance = Vector3.Distance(transform.position, lamp.transform.position);

            // Check if the distance is within 10 meters
            if (distance <= maxDistance)
            {

                // Calculate direction towards the orb
                Vector3 direction = (lamp.transform.position - transform.position).normalized;

                // Accelerate towards the orb
                transform.position += direction * accelerationSpeed * Time.deltaTime * (1 / distance + baseSpeed);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object collides with the orb
        if (other.gameObject == lamp)
        {
            // Despawn this object
            Destroy(gameObject);

            gameLogic.player.collectedLight += 1; // Increase the player's light by 1
        }
    }
}

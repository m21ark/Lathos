using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightOrbBehavior : MonoBehaviour
{
    public GameObject lamp;
    public float accelerationSpeed = 40f; // Speed at which the object accelerates towards the lamp
    public float maxDistance = 40f; // Distance at which the object starts to accelerate towards the lamp
    public float baseSpeed = 0.5f;
    private int lightValue = 2;

    private void Start()
    {
        lamp = GameObject.FindWithTag("Lamp");
    }

    private void Update()
    {
        if(lamp == null)         
            lamp = GameObject.FindWithTag("Lamp");
        else {
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
        if (other.gameObject.CompareTag("Lamp"))
        {
            Destroy(gameObject);
            GameLogic.instance.player.IncrementLight(lightValue);
        }
    }
}

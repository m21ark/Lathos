using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireplaceBehavior : MonoBehaviour
{

    public bool isLit = false;
    public int healAmount = 1;
    public float lightLimit = 20;
    public string dialogueName = "";
    public GameObject FireLightObj;

    private void RepelEnemies(Collider other)
    {
        if (other.CompareTag("Minion"))
        {
            Vector3 directionToCenter = transform.position - other.transform.position;
            directionToCenter.Normalize();
            other.GetComponent<Rigidbody>().AddForce(-directionToCenter * 10f, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameLogic.instance.StartDialogue(dialogueName);

            if(!isLit){
                isLit = true;
                FireLightObj.SetActive(true);
                GetComponent<Renderer>().material.color = new Color(1f, 0.5f, 0f);
            }
        }           
            
       RepelEnemies(other);
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            ProtoClass player = other.GetComponent<ProtoClass>();

            player.Heal(healAmount);

            if(player.collectedLight < lightLimit)
                player.collectedLight += 1;            
        }
        RepelEnemies(other);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireplaceBehavior : MonoBehaviour{
    private bool isLit = false;
    public GameObject FireLightObj;

    public string dialogueOnEnterID;

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player") && !isLit){
            isLit = true;
            FireLightObj.SetActive(true);

            if(dialogueOnEnterID != null)
                DialogueController.instance.StartDialogue(dialogueOnEnterID);
        }           
        RepelEnemies(other);
    }

    private void OnTriggerStay(Collider other){
        if (other.CompareTag("Player")){
            ProtoClass player = other.GetComponent<ProtoClass>();

            // Restore health and increase light of player
            player.Heal(1);
            if(player.collectedLight < 50)
                player.IncrementLight(1);        
        }
        RepelEnemies(other);
    }

    private void RepelEnemies(Collider other){
       if (other.CompareTag("Mob"))
        {
            Vector3 directionToCenter = transform.position - other.transform.position;
            directionToCenter.Normalize();
            other.GetComponent<Rigidbody>().AddForce(-directionToCenter * 10f, ForceMode.Impulse);
        }
    }
}

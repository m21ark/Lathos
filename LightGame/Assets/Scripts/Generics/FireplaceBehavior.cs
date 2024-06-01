using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireplaceBehavior : MonoBehaviour{
    private bool isLit = false;
    public GameObject FireLightObj;

    public string dialogueOnEnterID;

    public int fadeOutTime = 0;

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player") && !isLit){
            isLit = true;
            FireLightObj.SetActive(true);

            if(fadeOutTime != 0)
                StartCoroutine(FadeOut());

            if(dialogueOnEnterID != null && dialogueOnEnterID.Trim() != "" ){

                if(dialogueOnEnterID.Contains("opening"))
                    DialogueController.instance.StartOpeningWithSound(dialogueOnEnterID);
                else
                    DialogueController.instance.StartDialogue(dialogueOnEnterID);
            }
        }           
        // RepelEnemies(other);
    }

    private void OnTriggerStay(Collider other){
        if (other.CompareTag("Player") && FireLightObj.activeSelf){
            ProtoClass player = other.GetComponent<ProtoClass>();

            // Restore health and increase light of player
            player.Heal(1);
            player.IncrementLight(1);        
        }
        // RepelEnemies(other);
    }

    private IEnumerator FadeOut(){
        yield return new WaitForSeconds(fadeOutTime);
        FireLightObj.SetActive(false);
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

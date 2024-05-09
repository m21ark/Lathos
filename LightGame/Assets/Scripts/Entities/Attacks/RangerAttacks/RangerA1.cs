using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RangerA1 : ProtoAttack
{
     // on collision, attach a new script to the object
     public override void OnTriggerEnter(Collider collision)
     {
         if(ignorePlayerCol && (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Lamp")))
             return;
         
        // Check if the attack hit enemy and damage it
        if (collision.gameObject.CompareTag("Minion") || collision.gameObject.CompareTag("Boss"))
        {
           /// Get the Mob component
            ProtoMob mob = collision.gameObject.GetComponent<ProtoMob>();

            mob.TakeDamage(projDamage); 

            if (!collision.gameObject.GetComponent<RangerMark>())
                collision.gameObject.AddComponent<RangerMark>();
            else collision.gameObject.GetComponent<RangerMark>().refreshTicks();    
        }
        Destroy(gameObject.transform.parent.gameObject);
     }
}

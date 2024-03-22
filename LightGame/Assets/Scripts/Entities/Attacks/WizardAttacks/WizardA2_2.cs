using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardA2_2 : ProtoAttack
{

    private float lastTick = 0f;
    public float tickRate = 0.5f;

    public void OnTriggerStay(Collider collision)
    {
        Debug.Log(lastTick);
        lastTick -= Time.deltaTime;
        //only do damage if lastTick >= tickRate
        if (Time.time - lastTick >= tickRate)
        {
            lastTick = Time.time;
        }
        else
        {
            return;
        }
        
        if (collision.gameObject.CompareTag("Minion") || collision.gameObject.CompareTag("Boss"))
        {
            /// Get the Mob component
            ProtoMob mob = collision.gameObject.GetComponent<ProtoMob>();

            mob.TakeDamage(projDamage); 

            return;

        }
        
    }

    public override void OnTriggerEnter(Collider collision)
    {

    }
}

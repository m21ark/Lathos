using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SorcererA0 : ProtoAttack
{
    // on collision, attach a new script to the object
    public override void OnTriggerEnter(Collider collision)
    {
        if (ignorePlayerCol && (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Lamp")))
            return;

        // Check if the attack hit enemy and damage it
        if (collision.gameObject.CompareTag("Mob") || collision.gameObject.CompareTag("Boss"))
        {
            /// Get the Mob component
            ProtoMob mob = collision.gameObject.GetComponent<ProtoMob>();

            mob.TakeDamage(projDamage);

            if (!collision.gameObject.GetComponent<SorcererTag>())
                collision.gameObject.AddComponent<SorcererTag>();
            else collision.gameObject.GetComponent<SorcererTag>().addStack();
        }
        Destroy(gameObject.transform.parent.gameObject);
    }
}

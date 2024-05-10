using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpshooterA2 : ProtoAttack
{


    public override void OnTriggerEnter(Collider collision)
    {
        if (ignorePlayerCol && (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Lamp")))
            return;

        if (collision.gameObject.CompareTag("Minion") || collision.gameObject.CompareTag("Boss"))
        {
            /// Get the Mob component
            ProtoMob mob = collision.gameObject.GetComponent<ProtoMob>();
            mob.TakeDamage(projDamage);

            return;

        }

        if (despawnOnCol)
            Destroy(gameObject.transform.parent.gameObject);

    }
}

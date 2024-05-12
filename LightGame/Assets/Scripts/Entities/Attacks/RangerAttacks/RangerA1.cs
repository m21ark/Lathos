using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RangerA1 : ProtoAttack
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

            if (!collision.gameObject.GetComponent<RangerMark>())
            {
                int ticks = (int)GetKwarg("ticks", this.kwargs);
                int damageTick = (int)GetKwarg("damage", this.kwargs);

                collision.gameObject.AddComponent<RangerMark>();
                collision.gameObject.GetComponent<RangerMark>().setValues(ticks, damageTick);
            }
            else collision.gameObject.GetComponent<RangerMark>().refreshTicks();
        }
        Destroy(gameObject.transform.parent.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpshooterA2 : ProtoAttack
{


    public override void OnTriggerEnter(Collider collision)
    {
        if (ignorePlayerCol && (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Lamp")))
            return;

        if (collision.gameObject.CompareTag("Mob") || collision.gameObject.CompareTag("Boss"))
        {
            /// Get the Mob component
            ProtoMob mob = collision.gameObject.GetComponent<ProtoMob>();
            mob.TakeDamage(projDamage);

            return;

        }

        if (despawnOnCol)
            Destroy(gameObject.transform.parent.gameObject);

    }

    public override void Fire(int damage, Vector3 direction, params (string key, object value)[] kwargs)
    {

        this.kwargs = kwargs;

        // override the default prefab with the prefab from the kwargs
        //direction = (Vector3)GetKwarg("attackDirection", kwargs);

        attackRb = gameObject.GetComponent<Rigidbody>();
        if (attackRb != null) attackRb.velocity = direction * speed;

        projDamage = damage;

        // Despawn attack when despawn time is reached
        Destroy(gameObject.transform.parent.gameObject, despawnTime);
    }

}

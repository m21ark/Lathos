using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkerA0 : ProtoAttack
{

    public override void Fire(int damage = 10, Vector3 direction = default(Vector3), float speed = 200.0f, float gravity = 0.0f, float despawnTime = 7.5f){


        base.Fire(damage, direction, speed: 0.0f, despawnTime: 0.1f);

    }

    public override void OnTriggerEnter(Collider collision)
    {
        // Check if the attack is touching the ground
        if (collision.gameObject.CompareTag("Minion") || collision.gameObject.CompareTag("Boss"))
        {
            ProtoMob mob = collision.gameObject.GetComponent<ProtoMob>();

            mob.TakeDamage(projDamage); 

        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwing : ProtoAttack
{
    public override void Fire(int damage = 10, Vector3 direction = default(Vector3), float speed = 200.0f, float gravity = 0.0f, float despawnTime = 7.5f){
        base.Fire(damage, direction, speed: 0.0f, despawnTime: 0.2f);
    }
}

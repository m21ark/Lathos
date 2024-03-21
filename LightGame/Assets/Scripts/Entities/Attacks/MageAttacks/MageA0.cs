using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageA0 : ProtoAttack
{

    public override void Fire(int damage = 10, Vector3 direction = default(Vector3), float speed = 200.0f, float gravity = 0.0f, float despawnTime = 7.5f, GameObject prefab = null){
            base.Fire(damage, direction, speed: 50.0f, despawnTime: 7.5f);
    }


}

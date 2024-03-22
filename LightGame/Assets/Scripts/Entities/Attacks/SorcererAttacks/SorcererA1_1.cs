using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SorcererA1_1 : ProtoAttack
{

    private GameObject A1_2Prefab;
    private int A1_2Damage = 10;

    public override void Fire(int damage = 10, Vector3 direction = default(Vector3), float speed = 200.0f, float gravity = 0.0f, float despawnTime = 7.5f, GameObject prefab = null){
            base.Fire(0, direction, speed: 50.0f, gravity:gravityStrength, despawnTime: 7.5f);
            A1_2Prefab = prefab;
            A1_2Damage = damage;
    }

    public override void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            return;
        }
        
        GameObject attack = Instantiate(A1_2Prefab, transform.position, Quaternion.identity);
        ProtoAttack protoAttack = attack.transform.GetChild(0).GetComponent<ProtoAttack>();
        protoAttack.Fire(A1_2Damage, transform.forward, 0.0f, 0.0f, 0.1f);

        Destroy(gameObject.transform.parent.gameObject);
        
    }

}

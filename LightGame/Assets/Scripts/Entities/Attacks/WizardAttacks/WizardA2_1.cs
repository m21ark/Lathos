using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardA2_1 : ProtoAttack
{

    private GameObject A2_2Prefab;
    private int A2_2Damage = 10;

    public override void Fire(int damage, Vector3 direction, params (string key, object value)[] kwargs){
            base.Fire(0, direction);
            A2_2Prefab = (GameObject)GetKwarg("prefab", kwargs);
            A2_2Damage = damage;
    }

    public override void OnTriggerEnter(Collider collision)
    {
        if(ignorePlayerCol && (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Lamp")))
            return;
        
        if(!collision.gameObject.CompareTag("Floor")){
            return;
        }
        GameObject attack = Instantiate(A2_2Prefab, transform.position, Quaternion.identity);
        ProtoAttack protoAttack = attack.transform.GetChild(0).GetComponent<ProtoAttack>();
        protoAttack.Fire(A2_2Damage, transform.forward);

        Destroy(gameObject.transform.parent.gameObject);
        
    }
}

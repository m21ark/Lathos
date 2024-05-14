using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardA2_1 : ProtoAttack
{

    private GameObject A2_2Prefab;
    private GameObject A2_2VFX;
    private int A2_2Damage = 10;

    public override void Fire(int damage, Vector3 direction, params (string key, object value)[] kwargs)
    {
        despawnTime = 20.0f;
        base.Fire(0, direction);
        A2_2Prefab = (GameObject)GetKwarg("prefab", kwargs);
        A2_2VFX = (GameObject)GetKwarg("vfx", kwargs);
        A2_2Damage = damage;
    }

    public override void OnTriggerEnter(Collider collision)
    {
        if (ignorePlayerCol && (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Lamp")))
            return;

        if (!collision.gameObject.CompareTag("Floor") && !collision.gameObject.CompareTag("Boss") && !collision.gameObject.CompareTag("Mob"))
        {
            return;
        }
        Vector3 currentPos = transform.position;
        GameObject vfxInstance = Instantiate(A2_2VFX, transform.position, Quaternion.identity);
        vfxInstance.SetActive(true);
        Destroy(vfxInstance, 15.0f);
        
        StartCoroutine(GenerateA2_2(currentPos));
    }

    // Coroutine to generate A2_2 attack after a delay
    private IEnumerator GenerateA2_2(Vector3 currentPos)
    {
        yield return new WaitForSeconds(6.6f);
        GameObject attack = Instantiate(A2_2Prefab, currentPos, Quaternion.identity);
        ProtoAttack protoAttack = attack.transform.GetChild(0).GetComponent<ProtoAttack>();
        protoAttack.despawnTime = 4.2f;
        protoAttack.Fire(A2_2Damage, transform.forward);
    }
}

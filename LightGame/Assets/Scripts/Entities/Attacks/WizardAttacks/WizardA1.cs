using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardA1 : ProtoAttack
{   
    public float damageInterval = 0.5f;
    private float timer = 0f;

    public void OnTriggerStay(Collider collision)
    {
        timer += Time.deltaTime;
        if (timer >= damageInterval)
        {
            OnTriggerEnter(collision);
            timer = 0f;
        }
    }
}

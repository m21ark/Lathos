using UnityEngine;
using System.Collections;

public class RogueClass : RangerClass
{
    public float A1TimeSpan = 3.0f;
    public float A1BuffMult = 0.5f;

    public override void Attack(){
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttack(A0Prefab, out attack, out attackDirection);
        attack.Fire(A0Damage, attackDirection);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerRogueA0, transform.position);
    }
    
    public override void BaseAbility(){
        StartCoroutine(ActivateBaseAbility());
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerRogueA1, transform.position);
    }

    private IEnumerator ActivateBaseAbility(){
        
        float originalAttackSpeed = attackSpeed;
        attackSpeed *= A1BuffMult;
        yield return new WaitForSeconds(A1TimeSpan);
        attackSpeed = originalAttackSpeed;
    }

    public override void SpecialAbility(){
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttack(A2Prefab, out attack, out attackDirection);
        attack.Fire(A2Damage, attackDirection);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerRogueA2, transform.position);
    }


}



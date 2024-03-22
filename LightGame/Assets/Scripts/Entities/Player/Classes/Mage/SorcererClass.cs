using UnityEngine;
using System.Collections;

public class SorcererClass : MageClass
{

    private ProtoAttack attack;
    private Vector3 attackDirection;
    public float A1TimeDelta = 0.8f;

    public override void Attack() 
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttackAim(A0Prefab, out attack, out attackDirection);
        attack.Fire(A0Damage, attackDirection);
    }

    
    public override void BaseAbility()
    {
        StartCoroutine(RepeatedFire(A1Damage, attackDirection));
    }


    IEnumerator RepeatedFire(int baseDamage, Vector3 attackDirection)
    {
        // Call Fire method initially
        GenerateAttackAim(A1Prefab, out attack, out attackDirection);
        attack.Fire(A1Damage, attackDirection, prefab: A1_2Prefab);

        // Wait for 0.2 seconds
        yield return new WaitForSeconds(A1TimeDelta);

        // Call Fire method again
        GenerateAttackAim(A1Prefab, out attack, out attackDirection);
        attack.Fire(A1Damage, attackDirection, prefab: A1_2Prefab);

        // Wait for 0.2 seconds
        yield return new WaitForSeconds(A1TimeDelta);

        // Call Fire method again
        GenerateAttackAim(A1Prefab, out attack, out attackDirection);
        attack.Fire(A1Damage, attackDirection, prefab: A1_2Prefab);
    }

    public override void SpecialAbility()
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttackAim(A2Prefab, out attack, out attackDirection);
        attack.Fire(A2Damage, attackDirection);
    }

}

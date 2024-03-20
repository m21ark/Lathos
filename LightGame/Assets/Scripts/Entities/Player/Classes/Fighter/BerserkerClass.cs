using UnityEngine;
using System.Collections;

public class BersekerClass : FighterClass
{    
    
    private ProtoAttack attack;
    private Vector3 attackDirection;

    public float timeBetweenAttacks = 0.8f;
    
    public override void BaseAbility()
    {
        StartCoroutine(RepeatedFire(A1Damage, attackDirection));

    }


    IEnumerator RepeatedFire(int baseDamage, Vector3 attackDirection)
    {
        // Call Fire method initially
        GenerateAttackAim(A1Prefab, out attack, out attackDirection);
        attack.Fire(baseDamage, attackDirection);

        // Wait for 0.2 seconds
        yield return new WaitForSeconds(timeBetweenAttacks);

        // Call Fire method again
        GenerateAttackAim(A1Prefab, out attack, out attackDirection);
        attack.Fire(baseDamage, attackDirection);

        // Wait for 0.2 seconds
        yield return new WaitForSeconds(timeBetweenAttacks);

        // Call Fire method again
        GenerateAttackAim(A1Prefab, out attack, out attackDirection);
        attack.Fire(baseDamage, attackDirection);
    }



    public override void SpecialAbility()
    {
        // Special ability
    }
}


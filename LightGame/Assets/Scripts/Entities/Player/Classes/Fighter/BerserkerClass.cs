using UnityEngine;
using System.Collections;

public class BerserkerClass : FighterClass
{

    private ProtoAttack attack;
    private Vector3 attackDirection;

    public float A1TimeDelta = 0.8f;
    public float A2TimeSpan = 3.0f;
    public float A2BuffMult = 1.5f;
    public float A2DebuffMult = 0.5f;

    public override void Attack()
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttackPhysical(A0Prefab, out attack, out attackDirection);
        attack.Fire(A0Damage, cameraPivot.transform.forward);
    }

    public override void BaseAbility()
    {
        StartCoroutine(RepeatedFire(A1Damage, attackDirection));
    }


    IEnumerator RepeatedFire(int baseDamage, Vector3 attackDirection)
    {
        for (int i = 0; i < 3; i++)
        {
            // Call Fire method initially
            GenerateAttackPhysical(A1Prefab, out attack, out attackDirection);
            attack.Fire(baseDamage, cameraPivot.forward);

            // Wait for 0.2 seconds
            yield return new WaitForSeconds(A1TimeDelta);
        }
    }


    public override void SpecialAbility()
    {
        StartCoroutine(ActivateSpecialAbility());
    }

    private IEnumerator ActivateSpecialAbility()
    {
        // Double armor and base damage
        float originalArmor = armor;
        int originalBaseDamage = A0Damage;

        armor *= A2DebuffMult;
        A0Damage = Mathf.RoundToInt(A0Damage * A2BuffMult);

        yield return new WaitForSeconds(A2TimeSpan);

        // Restore original armor and base damage
        armor = originalArmor;
        A0Damage = originalBaseDamage;
    }
}


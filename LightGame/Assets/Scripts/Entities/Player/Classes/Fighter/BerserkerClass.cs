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

    public float A0StartOffset1 = 0.8f;
    public float A1StartOffset1 = 0.8f;

    public override void Attack()
    {
        DelayAttackPhysical(A0Prefab, A0Damage, A0StartOffset1);
    }

    public override void BaseAbility()
    {
        StartCoroutine(gameObject.GetComponent<PlayerController>().Dash());
        DelayAttackPhysical(A1Prefab, A1Damage, A1StartOffset1);
        //StartCoroutine(RepeatedFire(A1Damage, attackDirection));
    }

    IEnumerator RepeatedFire(int baseDamage, Vector3 attackDirection)
    {
        for (int i = 0; i < 3; i++)
        {
            // Call Fire method initially
            GenerateAttack(A1Prefab, out attack, out attackDirection);
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


using UnityEngine;
using System.Collections;

public class RogueClass : RangerClass
{


    public float A1TimeSpan = 3.0f;
    public float A1BuffMult = 0.5f;
    public override void Attack()
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttack(A0Prefab, out attack, out attackDirection);
        attack.Fire(A0Damage, attackDirection);
    }

    public override void BaseAbility()
    {
        StartCoroutine(ActivateBaseAbility());
    }

    private IEnumerator ActivateBaseAbility()
    {
        // Double armor and base damage
        float originalAttackSpeed = attackSpeed;
        attackSpeed *= A1BuffMult;

        yield return new WaitForSeconds(A1TimeSpan);

        // Restore original armor and base damage
        attackSpeed = originalAttackSpeed;
    }

    public override void SpecialAbility()
    {

        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttack(A2Prefab, out attack, out attackDirection);
        attack.Fire(A2Damage, attackDirection);
    }


}



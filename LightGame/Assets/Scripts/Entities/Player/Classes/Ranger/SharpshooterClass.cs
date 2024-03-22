using UnityEngine;
using System.Collections;

public class SharpshooterClass : RangerClass
{
    
    public float A1TimeSpan = 3.0f;
    public float A1BuffMult = 1.5f;
    public override void Attack() 
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttackAim(A0Prefab, out attack, out attackDirection);
        attack.Fire(A0Damage, attackDirection); 
    }

    public override void BaseAbility()
    {
        StartCoroutine(ActivateBaseAbility());
    }

    private IEnumerator ActivateBaseAbility()
    {
        // Double armor and base damage
        int originalA0Damage = A0Damage;
        int originalA2Damage = A2Damage;

        A0Damage = Mathf.RoundToInt(A0Damage * A1BuffMult);
        A2Damage = Mathf.RoundToInt(A2Damage * A1BuffMult);

        yield return new WaitForSeconds(A1TimeSpan);

        // Restore original armor and base damage
        A0Damage = originalA0Damage;
        A2Damage = originalA2Damage;
    }

    public override void SpecialAbility()
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttackAim(A2Prefab, out attack, out attackDirection);
        attack.Fire(A2Damage, attackDirection);
    }
}

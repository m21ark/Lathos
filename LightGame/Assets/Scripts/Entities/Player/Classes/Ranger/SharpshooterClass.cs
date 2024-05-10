using UnityEngine;
using System.Collections;

public class SharpshooterClass : RangerClass
{
    public GameObject VFXAbility;
    public GameObject VFXSpecialAbility;
    public override void Attack()
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttackAim(A0Prefab, out attack, out attackDirection);
        attack.Fire(A0Damage, attackDirection);
    }

    public override void BaseAbility()
    {
        GenerateVFX(VFXAbility, 15);
    }

    public override void SpecialAbility()
    {
        Vector3 offset = new Vector3(0, 0, 3);
        GenerateVFXOnPlayer(VFXSpecialAbility, transform, 15, offset);
        StartCoroutine(ActivateSpecialAbility());

    }

    private IEnumerator ActivateSpecialAbility()
    {
        yield return new WaitForSeconds(1.9f);
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttackAim(A2Prefab, out attack, out attackDirection);
        attack.Fire(A2Damage, attackDirection);
    }
}

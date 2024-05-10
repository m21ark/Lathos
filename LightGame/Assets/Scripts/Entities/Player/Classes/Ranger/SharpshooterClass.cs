using UnityEngine;
using System.Collections;

public class SharpshooterClass : RangerClass
{
    public GameObject VFXAbility;
    public override void Attack() 
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttackAim(A0Prefab, out attack, out attackDirection);
        attack.Fire(A0Damage, attackDirection); 
    }

    public override void BaseAbility()
    {
       GenerateVFXOnPlayer(VFXAbility, transform, 15);
    }

    public override void SpecialAbility()
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttackAim(A2Prefab, out attack, out attackDirection);
        attack.Fire(A2Damage, attackDirection);
    }
}

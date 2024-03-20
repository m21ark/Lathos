using UnityEngine;

public class MageClass : BaseClass
{
    public override void BaseAbility()
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttackAim(classAttackPrefab, out attack, out attackDirection);
        attack.Fire(classDamage, attackDirection);
    }
}

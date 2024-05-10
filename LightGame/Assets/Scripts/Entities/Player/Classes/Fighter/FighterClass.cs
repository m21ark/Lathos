using UnityEngine;

public class FighterClass : BaseClass
{
    public override void Attack()
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttackPhysical(A0Prefab, out attack, out attackDirection);
        attack.Fire(A0Damage, attackDirection);
    }

    public override void BaseAbility()
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttackPhysical(A1Prefab, out attack, out attackDirection);
        attack.Fire(A1Damage, attackDirection);
    }
}

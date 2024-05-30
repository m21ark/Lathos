using UnityEngine;

public class RangerClass : BaseClass
{

    public int A1_HunterMarkDamage = 8;
    public int A1_HunterMarkTicks = 3;

    public override void Attack()
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttack(A0Prefab, out attack, out attackDirection);
        attack.Fire(A0Damage, attackDirection);
    }

    public override void BaseAbility()
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttack(A1Prefab, out attack, out attackDirection);
        attack.Fire(A1Damage, attackDirection, ("ticks", A1_HunterMarkTicks), ("damage", A1_HunterMarkDamage));
    }
}

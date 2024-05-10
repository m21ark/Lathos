using UnityEngine;

public class MageClass : BaseClass
{

    public GameObject A1_2Prefab;

    public override void Attack()
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttackAim(A0Prefab, out attack, out attackDirection);
        attack.Fire(A0Damage, attackDirection);
    }

    public override void BaseAbility()
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttackAim(A1Prefab, out attack, out attackDirection);
        attack.Fire(A1Damage, attackDirection, ("prefab", A1_2Prefab));
    }
}

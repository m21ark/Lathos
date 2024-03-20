using UnityEngine;

public class FighterClass : BaseClass
{
    public override void BaseAbility()
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttackAim(A1Prefab, out attack, out attackDirection);
        attack.Fire(A1Damage, attackDirection);
    }
}

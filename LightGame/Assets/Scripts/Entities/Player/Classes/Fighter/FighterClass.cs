using UnityEngine;

public class FighterClass : BaseClass
{
    public override void BaseAbility()
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttackAim(classAttackPrefab, out attack, out attackDirection);
        attack.Fire(classDamage, attackDirection);  // hardcoded damage of 20 for now
    }
}

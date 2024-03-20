using UnityEngine;

public class BaseClass : ProtoClass
{
    public override void Attack() 
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttackAim(simpleAttackPrefab, out attack, out attackDirection);
        attack.Fire(baseDamage, attackDirection); // hardcoded damage of 10 for now
    }
}

using UnityEngine;

public class BaseClass : ProtoClass
{
    public override void Attack() 
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttackAim(A0Prefab, out attack, out attackDirection);
        attack.Fire(A0Damage, attackDirection); // hardcoded damage of 10 for now
    }
}

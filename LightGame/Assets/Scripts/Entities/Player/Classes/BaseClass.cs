using UnityEngine;

public class BaseClass : ProtoClass
{
    public override void Attack() 
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttackAim(simpleAttackPrefab, out attack, out attackDirection);
        attack.Fire(10, attackDirection, 200, 0.0f); // Hardcoded for now
    }
}

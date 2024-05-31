using UnityEngine;

public class BaseClass : ProtoClass
{
    public override void Attack()
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttack(A0Prefab, out attack, out attackDirection);
        attack.Fire(A0Damage, attackDirection);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerBaseA0, transform.position);
    }
}
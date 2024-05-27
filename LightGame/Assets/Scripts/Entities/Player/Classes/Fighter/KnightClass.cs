using UnityEngine;

public class KnightClass : FighterClass
{

    public override void Attack()
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttackPhysical(A0Prefab, out attack, out attackDirection);
        attack.Fire(A0Damage, attackDirection);

        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerSwordSwing, transform.position, 0.15f);
    }

    public override void BaseAbility()
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttackPhysical(A1Prefab, out attack, out attackDirection);
        attack.Fire(A1Damage, attackDirection);

        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerSwordSwing, transform.position, 0.2f);
    }

    public override void SpecialAbility()
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttackPhysical(A2Prefab, out attack, out attackDirection);
        attack.Fire(A2Damage, attackDirection);

        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerSwordSwing, transform.position, 0.5f);
    }
}

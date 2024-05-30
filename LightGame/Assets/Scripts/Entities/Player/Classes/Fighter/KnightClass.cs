using UnityEngine;

public class KnightClass : FighterClass
{
    public float A0StartOffset1 = 0.3f;
    public float A1StartOffset1 = 0.3f;
    public float A2StartOffset1 = 0.3f;

    public override void Attack()
    {
        DelayAttackPhysical(A0Prefab, A0Damage, A0StartOffset1);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerSwordSwing, transform.position, 0.15f);
    }

    public override void BaseAbility()
    {
        DelayAttackPhysical(A1Prefab, A1Damage, A1StartOffset1);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerSwordSwing, transform.position, 0.2f);
    }

    public override void SpecialAbility()
    {   
        DelayAttackPhysical(A2Prefab, A2Damage, A2StartOffset1);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerSwordSwing, transform.position, 0.5f);
    }
}

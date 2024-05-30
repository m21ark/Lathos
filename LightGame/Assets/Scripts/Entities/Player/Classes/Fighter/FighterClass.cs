using UnityEngine;

public class FighterClass : BaseClass
{
    public float A0StartOffset = 0.3f;
    public float A1StartOffset = 0.3f;

    public override void Attack()
    {
        DelayAttackPhysical(A0Prefab, A0Damage, A0StartOffset);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerSwordSwing, transform.position, 0.15f);
    }

    public override void BaseAbility()
    {
        DelayAttackPhysical(A1Prefab, A1Damage, A1StartOffset);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerSwordSwing, transform.position, 0.15f);
    }
}

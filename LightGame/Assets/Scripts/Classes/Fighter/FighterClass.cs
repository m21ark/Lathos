using UnityEngine;
public class FighterClass : BaseClass
{
    public new float armor = 1.2f;
    public override void Attack(ProtoProjectile projectile)
    {
        projectile.Fire();
    }

    public override void BaseAbility()
    {
        // Base ability
    }
}

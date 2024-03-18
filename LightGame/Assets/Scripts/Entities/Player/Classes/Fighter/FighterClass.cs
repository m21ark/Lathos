using UnityEngine;
public class FighterClass : BaseClass
{
    public override void Attack(ProtoProjectile projectile)
    {
        projectile.Fire();
    }

    public override void BaseAbility()
    {
        // Base ability
    }
}

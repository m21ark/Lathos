using UnityEngine;

public class SharpshooterClass : RangerClass
{
    public override void Attack(ProtoProjectile projectile)
    {
        projectile.Fire();
    }

    public override void BaseAbility()
    {
        // Base ability
    }

    public override void SpecialAbility()
    {
        // Special ability
    }
    
}

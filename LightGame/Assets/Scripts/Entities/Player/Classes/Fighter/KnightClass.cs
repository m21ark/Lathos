using UnityEngine;

public class KnightClass : FighterClass
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

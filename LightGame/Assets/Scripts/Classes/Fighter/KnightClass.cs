using UnityEngine;

public class KnightClass : FighterClass
{
    
    public new int damage = 15;
    public new float armor = 1.8f;

    public override void Attack(Projectile projectile)
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

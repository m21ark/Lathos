using UnityEngine;

public class SharpshooterClass : RangerClass
{
    
    public new int damage = 20;
    public float attackSpeed = 0.6f;

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

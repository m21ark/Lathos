using UnityEngine;

public class SorcererClass : MageClass
{
    
    public new int damage = 15;
    public float attackSpeed = 1.2f;

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

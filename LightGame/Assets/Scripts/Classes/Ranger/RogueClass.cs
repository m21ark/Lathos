using UnityEngine;

public class RogueClass : RangerClass
{
    
    public new int damage = 10;
    public float attackSpeed = 1.4f;

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


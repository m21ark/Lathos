using UnityEngine;

public class WizardClass : MageClass
{
    
    public new int damage = 30;
    public float attackSpeed = 0.4f;

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

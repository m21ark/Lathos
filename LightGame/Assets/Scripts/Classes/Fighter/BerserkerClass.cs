using UnityEngine;


public class BersekerClass : FighterClass
{
    
    public new int damage = 20;
    public new float armor = 1.0f;

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


using UnityEngine;

public class RangerClass : BaseClass
{
    public new float armor = 0.8f;
    public override void Attack(Projectile projectile)
    {
        projectile.Fire();
    }

    public override void BaseAbility()
    {
        // Base ability
    }
}

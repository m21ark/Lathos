using UnityEngine;

public class RangerClass : BaseClass
{
    public override void Attack(Projectile projectile)
    {
        projectile.Fire();
    }

    public override void BaseAbility()
    {
        // Base ability
    }
}

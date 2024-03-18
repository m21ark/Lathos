using UnityEngine;

public class RangerClass : BaseClass
{
    public override void Attack(ProtoProjectile projectile)
    {
        projectile.Fire();
    }

    public override void BaseAbility()
    {
        // Base ability
    }


}

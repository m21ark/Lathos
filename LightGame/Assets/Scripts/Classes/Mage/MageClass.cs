using UnityEngine;

public class MageClass : BaseClass
{

    public new float armor = 0.5f;
    public override void Attack(Projectile projectile)
    {
        projectile.Fire();
    }

    public override void BaseAbility()
    {
        // Base ability
    }
}

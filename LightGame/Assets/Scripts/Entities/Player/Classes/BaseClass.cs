using UnityEngine;

public class BaseClass : ProtoClass
{
  
    public override void Attack(ProtoProjectile projectile)
    {
        projectile.Fire();
    }  
}

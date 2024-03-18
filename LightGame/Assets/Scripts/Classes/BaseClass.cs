using UnityEngine;

public class BaseClass : ProtoClass
{
  
    public override void Attack(Projectile projectile)
    {
        projectile.Fire();
    }   
}

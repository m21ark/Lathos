using UnityEngine;

public class BaseClass : ProtoClass
{
  
    public override void Attack(Projectile projectile)
    {
        projectile.Fire();
    }  

    public override void InitializeAttributes(ClassAttribLoader loader)
    {
        base.InitializeAttributes(loader);

        if (loader.classAttributesDict.ContainsKey("BaseClass"))
        {
            var attributes = loader.classAttributesDict["BaseClass"];
            
            if (attributes.ContainsKey("health"))
                health = int.Parse(attributes["health"]);
            if (attributes.ContainsKey("damage"))
                damage = int.Parse(attributes["damage"]);
            if (attributes.ContainsKey("moveSpeed"))
                moveSpeed = float.Parse(attributes["moveSpeed"]);
            if (attributes.ContainsKey("armor"))
                armor = float.Parse(attributes["armor"]);
            if (attributes.ContainsKey("attackSpeed"))
                attackSpeed = float.Parse(attributes["attackSpeed"]);
        }
    } 
}

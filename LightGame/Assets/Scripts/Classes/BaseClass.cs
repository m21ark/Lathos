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
            
            if (attributes.ContainsKey("health") && attributes["health"] != "X")
                health = int.Parse(attributes["health"]);

            if (attributes.ContainsKey("damage")  && attributes["damage"] != "X")
                damage = int.Parse(attributes["damage"]);

            if (attributes.ContainsKey("moveSpeed")  && attributes["moveSpeed"] != "X")
                moveSpeed = float.Parse(attributes["moveSpeed"] );


            if (attributes.ContainsKey("armor") && attributes["armor"] != "X")
                armor = float.Parse(attributes["armor"]);

            
            if (attributes.ContainsKey("attackSpeed") && attributes["attackSpeed"] != "X")
                attackSpeed = float.Parse(attributes["attackSpeed"]);
        }
    } 

    
}

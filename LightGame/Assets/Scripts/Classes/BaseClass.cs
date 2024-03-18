using UnityEngine;

public class BaseClass : ProtoClass
{
  
    public override void Attack(Projectile projectile)
    {
        projectile.Fire();
    }  

    public override void InitializeAttributes(ClassAttribLoader loader, string className)
    {
        base.InitializeAttributes(loader);

        if (loader.classAttributesDict.ContainsKey(className))
        {
            var attributes = loader.classAttributesDict[className];
            
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

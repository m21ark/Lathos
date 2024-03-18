using UnityEngine;

public class RogueClass : RangerClass
{
    
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

    public override void InitializeAttributes(ClassAttribLoader loader)
    {
        base.InitializeAttributes(loader);

        if (loader.classAttributesDict.ContainsKey("Rogue"))
        {
            var attributes = loader.classAttributesDict["Rogue"];
            if (attributes.ContainsKey("damage"))
                damage = int.Parse(attributes["damage"]);
            if (attributes.ContainsKey("attackSpeed"))
                attackSpeed = float.Parse(attributes["attackSpeed"]);
        }
    }
}



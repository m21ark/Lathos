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

        public override void InitializeAttributes(ClassAttribLoader loader)
    {
        base.InitializeAttributes(loader);

        if (loader.classAttributesDict.ContainsKey("Ranger"))
        {
            var attributes = loader.classAttributesDict["Ranger"];
            if (attributes.ContainsKey("armor"))
                armor = float.Parse(attributes["armor"]);
        }
    }
}

using UnityEngine;

public class ProtoClass : MonoBehaviour
{
    public int health;
    public int damage;
    public float moveSpeed;
    public float armor;
    public float attackSpeed;

    public virtual void InitializeAttributes(ClassAttribLoader loader, string[] classNames)
    {
        foreach (string className in classNames)
        {
            if (loader.classAttributesDict.ContainsKey(className))
            {
                var attributes = loader.classAttributesDict[className];

                if (attributes.ContainsKey("health") && attributes["health"] != "X")
                    health = int.Parse(attributes["health"]);

                if (attributes.ContainsKey("damage") && attributes["damage"] != "X")
                    damage = int.Parse(attributes["damage"]);

                if (attributes.ContainsKey("moveSpeed") && attributes["moveSpeed"] != "X")
                    moveSpeed = float.Parse(attributes["moveSpeed"]);

                if (attributes.ContainsKey("armor") && attributes["armor"] != "X")
                    armor = float.Parse(attributes["armor"]);

                if (attributes.ContainsKey("attackSpeed") && attributes["attackSpeed"] != "X")
                    attackSpeed = float.Parse(attributes["attackSpeed"]);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // Die
        Destroy(gameObject);
    }

    public virtual void Attack(ProtoProjectile projectile)
    {
        projectile.Fire();
    }   

    public virtual void BaseAbility()
    {
        // Base ability
    }

    public virtual void SpecialAbility()
    {
        // Special ability
    }

}

using UnityEngine;

public class ProtoClass : MonoBehaviour
{
    public int health = 100;
    public int damage = 10;
    public float moveSpeed = 5f;
    public float armor = 1f;
    public float attackSpeed = 1.0f;


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

    public virtual void Attack(Projectile projectile)
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

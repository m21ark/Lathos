using UnityEngine;

public class ProtoClass: MonoBehaviour
{
    public int health;
    public int damage;
    public float moveSpeed;
    public float armor;
    public float attackSpeed;

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

    public virtual void InitializeAttributes(ClassAttribLoader loader)
    {
        // Load common attributes here
    }
}

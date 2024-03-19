using UnityEngine;

public class ProtoClass : MonoBehaviour
{

    // Camera / Movement Related
    public float moveSpeed = 8f;
    public float jumpForce = 10f;
    public float dashCooldown = 1f;
    private Transform cameraPivot;

    // In-game Attributes
    public int maxHealth = 100;
    public int health = 100;
    public int damage = 10;
    public float armor = 1;
    public int collectedLight = 0;

    // Attack reload time
    public float basicAttackReloadTime = 0.3f;
    public float baseAttackReloadTime = 1f;
    public float abilityAttackReloadTime = 2f;

    // Attack charge rate to launch
    public float basicAttackRate = 0f;
    public float baseAttackRate = 0f;
    public float abilityAttackRate = 0f;

    // Attack prefabs
    public GameObject simpleAttackPrefab;
    public GameObject classAttackPrefab;
    public GameObject specialAttackPrefab;

    void Start(){
        cameraPivot = transform.Find("CameraPivot");
    }

    public bool isAlive(){
        return health > 0;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            Die();
    }

    public GameObject getGameObject(){
        return gameObject;
    }

    public void Heal(int heal){
        health += heal;
        if(health > maxHealth) health = maxHealth;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public virtual void Attack() // Vector3 position = default(Vector3), Vector3 direction = default(Vector3)
    {
        Debug.Log("Basic Attack");
    }   

    public virtual void BaseAbility()
    {
        Debug.Log("Main Base Attack");
    }

    public virtual void SpecialAbility()
    {
        Debug.Log("Special Attack");
    }

}

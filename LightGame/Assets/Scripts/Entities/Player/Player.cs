using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float jumpForce = 10f;

    public int maxHealth = 100;
    public int health = 100;
    public new int light = 0;

    public string playerClass = "Base";

    public bool isAlive()
    {
        return health > 0;
    }

    public GameObject getGameObject(){
        return gameObject;
    }

    public void TakeDamage(int damage){
        health -= damage;
    }

    public void Heal(int heal){
        health += heal;
        if(health > maxHealth)
            health = maxHealth;
    }
}

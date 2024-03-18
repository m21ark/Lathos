using UnityEngine;

public class Player : MonoBehaviour
{
    // Movement Related
    public float moveSpeed = 8f;
    public float jumpForce = 10f;

    // Player Attributes
    public int maxHealth = 100;
    public int health = 100;
    public new int light = 0;

    // Class Related
    private string playerClassName = "Base";
    private BaseClass playerClass;
    private ClassAttribLoader classLoader;

    void Start(){
        loadClassInfo();
        setBaseClass();
    }

    public BaseClass getClass(){
        return playerClass;
    }

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

    void loadClassInfo(){
        // Load all class attributes
        classLoader = new ClassAttribLoader();
        classLoader.LoadAttributesFromCSV("Assets/Scripts/Entities/Player/Classes/Utils/ClassAttributes.csv");
    }

    void setBaseClass(){
        // Instantiate an empty GameObject and attach BaseClass component to it
        GameObject playerClassObject = new GameObject("PlayerClass");
        playerClass = playerClassObject.AddComponent<BaseClass>();

        // Initialize playerClass attributes
        string[] classNames = { "Base" };
        playerClass.InitializeAttributes(classLoader, classNames);
    }

    public void Attack(){
        BaseProjectile projectile = (new GameObject("Projectile")).AddComponent<BaseProjectile>();
        playerClass.Attack(projectile);
    }
}

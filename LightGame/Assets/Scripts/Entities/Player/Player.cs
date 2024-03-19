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

    // Attack rates / reload times
    public float basicAttackRate = 0.5f;
    public float baseAttackRate = 1f;
    public float abilityAttackRate = 2f;

    // Class Related
    // private string playerClassName = "Base";
    private BaseClass playerClass;
    private ClassAttribLoader classLoader;
    private Transform cameraPivot;

    void Start(){
        loadClassInfo();
        setBaseClass();
        cameraPivot = transform.Find("CameraPivot");
    }

    public Transform getCameraPivot(){
        return cameraPivot;
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
        playerClass.Attack(cameraPivot.forward, transform.position);
    }

    public void BaseAbility(){
        Debug.Log("Main Base Attack");
    }

    public void SpecialAbility(){
        Debug.Log("Special Attack");
    }

}

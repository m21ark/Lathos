using UnityEngine;

public class ProtoClass : MonoBehaviour
{

    // Camera / Movement Related
    public float moveSpeed = 8f;
    public float jumpForce = 10f;
    public float dashCooldown = 1f;
    public float dashSpeed = 25f;
    protected Transform cameraPivot;

    // In-game Attributes
    public int maxHealth = 100;
    public int health = 100;
    public float armor = 1;



    // A0 - Basic Attack
    public GameObject A0Prefab;
    public int A0Damage = 10;
    public float A0ReloadTime = 0.3f;
    public float A0ChargeRate = 0f;


    // A1 - Ability 1
    public GameObject A1Prefab;
    public int A1Damage = 20;
    public float A1ReloadTime = 1f;
    public float A1ChargeRate = 0f;


    // A2 - Ability 2
    public GameObject A2Prefab;
    public int A2Damage = 30;
    public float A2ReloadTime = 2f;
    public float A2ChargeRate = 0f;



    public int collectedLight = 0;

    // Fire rate / reload controls
    [HideInInspector] public float lastAttackTime = 0f;
    [HideInInspector] public float lastAttack1Time = 0f;
    [HideInInspector] public float lastAttack2Time = 0f;
    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool isAttack1ing = false;
    [HideInInspector] public bool isAttack2ing = false;
    [HideInInspector] public float lastDashTime = 0f;


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
        // Don't destroy the player object because it holds the camera 
    }

    public virtual void Attack() // Vector3 position = default(Vector3), Vector3 direction = default(Vector3)
    {
        Debug.Log("Basic Attack is not implemented for this player class");
    }   

    public virtual void BaseAbility()
    {
        Debug.Log("Main Base Attack is not implemented for this player class");
    }

    public virtual void SpecialAbility()
    {
        Debug.Log("Special Attack is not implemented for this player class");
    }

    public string getClassName(){
        System.Type scriptType = this.GetType();
        string className = scriptType.Name;
        if (className.EndsWith("Class"))
            className = className.Substring(0, className.Length - "Class".Length);
        return className;
    }

    public void GenerateAttackAim(GameObject prefab, out ProtoAttack attack, out Vector3 attackDirection){
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        Vector3 attackDirectionTemp;
        RaycastHit ray;

        bool hit = Physics.Raycast(camera.transform.position, camera.transform.forward, out ray, 50f, ~LayerMask.GetMask("LampLight"));

        if(hit){
            attackDirectionTemp = ray.point - cameraPivot.position;
            attackDirectionTemp.Normalize();
        } else attackDirectionTemp = camera.transform.forward;

        Vector3 startPos = cameraPivot.transform.position + cameraPivot.transform.forward;
        GameObject attackEntity = Instantiate(prefab, startPos, Quaternion.identity);
        ProtoAttack attackTemp = attackEntity.GetComponent<ProtoAttack>();

        // Return values
        attack = attackTemp;
        attackDirection = attackDirectionTemp;
    }
}

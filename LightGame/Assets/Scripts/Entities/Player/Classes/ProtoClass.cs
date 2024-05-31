using UnityEngine;
using System;
using System.Collections;


public class ProtoClass : MonoBehaviour
{

    // Camera / Movement Related
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float jumpForce = 10f;
    public float dashCooldown = 1f;
    public float dashSpeed = 25f;
    protected Transform cameraPivot;

    // Light / Energy
    [Header("Light")]
    public int collectedLight = 0;

    // In-game Attributes
    [Header("Attributes")]
    public int maxHealth = 100;
    public int health = 100;
    public float armor = 1;
    public float attackSpeed = 1f;

    // A0 - Basic Attack
    [Header("Attack A0")]
    public GameObject A0Prefab;
    public int A0Damage = 10;
    public float A0ReloadTime = 0.3f;
    public float A0ChargeRate = 0f;
    public bool stopsMovementA0 = false;

    // A1 - Ability 1
    [Header("Ability A1")]
    public GameObject A1Prefab;
    public int A1Damage = 20;
    public float A1ReloadTime = 1f;
    public float A1ChargeRate = 0f;
    public bool stopsMovementA1 = false;

    // A2 - Ability 2
    [Header("Ability A2")]
    public GameObject A2Prefab;
    public int A2Damage = 30;
    public float A2ReloadTime = 2f;
    public float A2ChargeRate = 0f;
    public bool stopsMovementA2 = false;

    // Fire rate / reload controls
    [HideInInspector] public float lastAttackTime = 0f;
    [HideInInspector] public float lastAttack1Time = 0f;
    [HideInInspector] public float lastAttack2Time = 0f;
    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool isAttack1ing = false;
    [HideInInspector] public bool isAttack2ing = false;
    [HideInInspector] public float lastDashTime = 0f;

    // Attack animation flags
    [HideInInspector] public bool pendingA0Animate = false;
    [HideInInspector] public bool pendingA1Animate = false;
    [HideInInspector] public bool pendingA2Animate = false;

    public bool hasPendingAnimation(int num)
    {
        bool aux = false;
        switch (num)
        {
            case 0:
                aux = pendingA0Animate;
                pendingA0Animate = false;
                break;
            case 1:
                aux =  pendingA1Animate;
                pendingA1Animate = false;
                break;
            case 2:
                aux = pendingA2Animate;
                pendingA2Animate = false;
                break;
            default:
                return false;
        }
        return aux;
    }

    void Start()
    {
        cameraPivot = transform.parent.transform.Find("CameraPivot");
        health = maxHealth;
        collectedLight = 100;
    }

    public bool isAlive()
    {
        return health > 0;
    }

    public void TakeDamage(int damage)
    {
        health -= Mathf.RoundToInt(damage / armor);
        if (health <= 0)
            Die();
    }

    public void Heal(int heal)
    {
        health += heal;
        if (health > maxHealth) health = maxHealth;
    }

    public void Die()
    {
        // Don't destroy the player object because it holds the camera 
    }

    public virtual void Attack()
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

    public string getClassName()
    {
        System.Type scriptType = this.GetType();
        string className = scriptType.Name;
        if (className.EndsWith("Class"))
            className = className.Substring(0, className.Length - "Class".Length);
        return className;
    }

    public void GenerateAttack(GameObject prefab, out ProtoAttack attack, out Vector3 attackDirection)
    {
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        Vector3 attackDirectionTemp;
        RaycastHit ray;

        bool hit = Physics.Raycast(camera.transform.position, camera.transform.forward, out ray, 50f, ~LayerMask.GetMask("LampLight"));

        if (hit)
        {
            attackDirectionTemp = ray.point - cameraPivot.position;
            attackDirectionTemp.Normalize();
        }
        else attackDirectionTemp = camera.transform.forward;

        GameObject attackEntity;
        Vector3 startPos = cameraPivot.transform.position + cameraPivot.transform.forward;

        attackEntity = Instantiate(prefab, startPos, cameraPivot.rotation);

        ProtoAttack attackTemp = attackEntity.transform.GetChild(0).GetComponent<ProtoAttack>();

        // Return values
        attack = attackTemp;
        attackDirection = attackDirectionTemp;
    }

    public void DelayAttackPhysical(GameObject prefab, int damage, float delay){
        StartCoroutine(DelayAttackLaunchEnum(prefab, damage, delay));
    }

    private IEnumerator DelayAttackLaunchEnum(GameObject prefab, int damage, float delay){
        ProtoAttack attack;
        Vector3 attackDirection;
        yield return new WaitForSeconds(delay);
        GenerateAttack(prefab, out attack, out attackDirection);
        Debug.Log("Attack direction: " + attackDirection);
        //attackDirection.y = 0;
        //attackDirection.Normalize();
        Debug.Log("Attack direction normalized: " + attackDirection);

        attack.transform.parent.transform.rotation = Quaternion.Euler(0, cameraPivot.transform.rotation.eulerAngles.y, 0);
        attack.Fire(damage, attackDirection);
    }

    // ============================== VFX ==============================

    public void GenerateVFX(GameObject vfx, int duration = 5)
    {
        GameObject vfxInstance = Instantiate(vfx, transform.position, Quaternion.identity);
        vfxInstance.SetActive(true);
        Destroy(vfxInstance, duration);
    }

    public void GenerateVFXOnPlayer(GameObject vfx, Transform playerTransform, int duration = 5, Vector3 offsetPosition = new Vector3(), Quaternion rotation = new Quaternion())
    {
        GameObject vfxInstance = Instantiate(vfx, playerTransform.position, Quaternion.identity);
        vfxInstance.transform.parent = playerTransform.transform;
        vfxInstance.transform.localPosition = offsetPosition;
        vfxInstance.transform.localRotation = rotation;
        vfxInstance.SetActive(true);
        Destroy(vfxInstance, duration);
    }

    private IEnumerator generateVFXDelay(GameObject vfx, float delay, Action delayedAction, int duration = 5)
    {
        GenerateVFX(vfx, duration);
        yield return new WaitForSeconds(delay);
        delayedAction();
    }

    public void generateVFXDelayedAction(GameObject vfx, float delay, Action delayedAction, int duration = 5)
    {
        StartCoroutine(generateVFXDelay(vfx, delay, delayedAction, duration));
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ProtoMob : MonoBehaviour
{
    [Header("Health Information")]
    public int health = 40;
    public int maxHealth = 40;
    public int enemyLevel = 5; // affects orb drop rate
    public int damage = 20;

    public GameObject xpOrbPreFab;


    [Header("Components")]
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    // Patrolling
    [Header("Patrolling")]
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    [Header("Attacking")]
    public float timeBetweenAttacks;
    public bool alreadyAttacked;
    public GameObject projectile;

    // States
    [Header("States")]
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        //player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

        // Make the meshes stick to the ground

        //if does not have a first child then return immediately
        // if (transform.childCount == 0) return;

        // // Perform raycast from the object's position downwards
        // RaycastHit hit;
        // if (Physics.Raycast(transform.GetChild(0).position, Vector3.down, out hit, 10f))
        // {
        //     // Get the normal of the hit point
        //     Vector3 normal = hit.normal;

        //     // Calculate the rotation needed to be parallel to the ground
        //     Quaternion rotation = Quaternion.FromToRotation(transform.GetChild(0).up, normal) * transform.GetChild(0).rotation;

        //     // Apply the rotation
        //     transform.GetChild(0).rotation = Quaternion.Lerp(transform.GetChild(0).rotation, rotation, 0.1f);
        // }

    }

    protected virtual void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Debug.Log("Distance to walkpoint: " + distanceToWalkPoint.magnitude);
        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 3.5f) walkPointSet = false;
    }

    protected virtual void SearchWalkPoint()
    {
        // Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 12f, whatIsGround)) walkPointSet = true;

    }

    protected virtual void ChasePlayer()
    {

        agent.SetDestination(player.position);
    }


    public virtual void AttackPlayer()
    {
        agent.SetDestination(player.position);

        //transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Attack code here

            this.attack();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public virtual void attack()
    {
        Debug.Log("Attacking");
    }

    public virtual void ResetAttack()
    {
        alreadyAttacked = false;
    }


    public void Die()
    {
        // Randomly determine the number of orbs to spawn based on enemy level
        int numOrbs = Random.Range(enemyLevel, enemyLevel * 2);

        // Spawn orbs
        for (int i = 0; i < numOrbs; i++)
        {
            // Calculate random offset from the center
            float xOffset = Random.Range(-2f, 2f);
            float zOffset = Random.Range(-2f, 2f);
            Vector3 spawnPosition = gameObject.transform.position + new Vector3(xOffset, 0, zOffset);

            // Instantiate orb with slight offset
            GameObject orb = Instantiate(xpOrbPreFab, spawnPosition, Quaternion.identity);

            Destroy(orb, 10f); // Despawn orbs if not collected in 10s
        }

        // Destroy the enemy self GameObject
        Destroy(gameObject);
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }

}

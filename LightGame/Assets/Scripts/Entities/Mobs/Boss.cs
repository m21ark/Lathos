using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : ProtoMob
{
    // Phase Logic
    public Transform projectileSpawnPoint;

    private int currentBossPhase = 1;
    public float bossPhaseThreshold2Percentage = 0.5f; // 50% of max health
    public float bossPhaseThreshold3Percentage = 0.2f; // 20% of max health

    // Summoning Logic
    private float lastSummonTime;
    public float summonFrequency = 10.0f;
    [SerializeField] public GameObject[] minionPrefab;

    // Child visibility (accessed via hierarchy)
    private GameObject firstChild;
    private GameObject secondChild;

    public Transform leftWing;
    public Transform rightWing;


    private float rotationSpeed = 16.0f; // Speed of rotation
    private float upspeed = 0.5f;

    public float flapFrequency = 2f;
    public float flapAmplitude = 30f;

    private float flapTime;

    public Vector3 phase3TargetRotation = new Vector3(63.121f, 48.889f, -104.672f); // Target rotation in Euler angles

    private bool isJumping = false;
    private bool isLanding = false;

    void Start()
    {
        lastSummonTime = Time.time;

        firstChild = transform.GetChild(0).gameObject;
        secondChild = transform.GetChild(1).gameObject;

        // Ensure initial visibility state
        firstChild.SetActive(true);
        secondChild.SetActive(false);
    }

    new void Update()
    {
        // Check if boss needs to switch to new phase
        currentBossPhase = 1;
        float healthPercentage = (float)health / maxHealth;

        if (healthPercentage <= bossPhaseThreshold2Percentage) currentBossPhase = 2;
        if (healthPercentage <= bossPhaseThreshold3Percentage) currentBossPhase = 3;


        // Apply different boss behavior depending on current phase  
        switch (currentBossPhase)
        {
            case 1: Phase1Behavior(); break;
            case 2: Phase2Behavior(); break;
            case 3: Phase3Behavior(); break;
        }
    }

    protected void ShootProjectile()
    {
        if (projectile != null && player != null && projectileSpawnPoint != null)
        {
            Vector3 playerPos = player.position;
            playerPos.y += 0.8f;
            Vector3 direction = (playerPos - projectileSpawnPoint.position).normalized;

            Quaternion rotation = Quaternion.LookRotation(direction);
            rotation *= Quaternion.Euler(-90f, 0f, 0f);
            GameObject spawnedProjectile = Instantiate(projectile, projectileSpawnPoint.position, rotation);
            Rigidbody rb = spawnedProjectile.GetComponent<Rigidbody>();

            if (rb != null)
            {

                spawnedProjectile.GetComponent<Projectile>().Fire(10, direction);

            }
        }
    }

    private void Phase1Behavior()
    {

        // TODO: if player is close to the bettle then fly and to another position
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (isJumping)
        {
            // if rotation is similar to target rotation
            Debug.Log(Quaternion.Angle(secondChild.transform.rotation, Quaternion.Euler(phase3TargetRotation)));
            if (isLanding || Quaternion.Angle(secondChild.transform.rotation, Quaternion.Euler(phase3TargetRotation)) < 0.01f)
            {
                LerpToGroundPhase();
                isLanding = true && isJumping;
            }
            else
            {
                LerpToFlightPhase();
            }
            FlapWings();
            agent.baseOffset = Mathf.Lerp(agent.baseOffset, 15f, Time.deltaTime * upspeed);
            return;

        }
        else
        {
            // attack player 
            AttackPlayer();
        }

        if (distanceToPlayer < 10.0f)
        {
            Vector3 newPosition = transform.position + Random.insideUnitSphere * 80.0f;
            UnityEngine.AI.NavMeshHit hit;
            if (UnityEngine.AI.NavMesh.SamplePosition(newPosition, out hit, 30.0f, UnityEngine.AI.NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
            isJumping = true;
        }
        else
        {
            // secondChild.transform.LookAt(player);
            // firstChild.transform.LookAt(player);
        }

    }



    private void Phase2Behavior()
    {
        // Summon minion if 10s cooldown has passed
        if (Time.time - lastSummonTime >= summonFrequency) SummonMinions();

        LerpToFlightPhase();
        FlapWings();
    }

    private void Phase3Behavior()
    {
        // set agent offset to 10
        agent.baseOffset = Mathf.Lerp(agent.baseOffset, 15f, Time.deltaTime * upspeed);
        agent.radius = 1.0f;

        agent.SetDestination(player.position);

    }


    private void LerpToFlightPhase()
    {
        // You should make visible the second child and make invisible the first child
        if (firstChild != null && secondChild != null)
        {
            firstChild.SetActive(false);
            secondChild.SetActive(true);
        }

        Quaternion targetRotation = Quaternion.Euler(phase3TargetRotation);
        secondChild.transform.rotation = Quaternion.RotateTowards(secondChild.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void LerpToGroundPhase()
    {
        Quaternion targetRotation = Quaternion.Euler(36.753f, -4.393f, 0.866f);
        secondChild.transform.rotation = Quaternion.RotateTowards(secondChild.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        // Ensure the boss has landed (y position is back to original ground position)

        agent.baseOffset = Mathf.Lerp(agent.baseOffset, -0.23f, Time.deltaTime * upspeed * 20.0f);
        Debug.Log(Quaternion.Angle(targetRotation, secondChild.transform.rotation));
        if (Quaternion.Angle(targetRotation, secondChild.transform.rotation) < 0.01f && secondChild.transform.rotation.y > -4.393f
            && agent.baseOffset < 0.40f
        )  // Adding a small threshold to handle floating-point precision
        {
            agent.baseOffset = -0.23f;
            // Make the first child visible and the second child invisible
            if (firstChild != null && secondChild != null)
            {
                firstChild.SetActive(true);
                secondChild.SetActive(false);
            }
            secondChild.transform.LookAt(player);
            firstChild.transform.LookAt(player);

            isJumping = false;
        }
    }




    private void FlapWings()
    {
        flapTime += Time.deltaTime * flapFrequency;
        float flapAngle = Mathf.Sin(flapTime) * flapAmplitude;

        leftWing.localRotation = Quaternion.Euler(-150.798f, flapAngle + 12.446f, 48.228f);
        rightWing.localRotation = Quaternion.Euler(29.202f, -flapAngle - 12.446f, 48.228f);
    }


    public void Move2()
    {
        agent.SetDestination(player.position);
    }

    // void OnCollisionEnter(Collision collision)
    // {
    //     // Deal Damage ... TODO: maybe change this to put like ant
    //     if (collision.gameObject.CompareTag("Player"))
    //         GameLogic.instance.player.TakeDamage(damage);
    // }

    public override void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        // Look at the player
        // transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Attack code here
            ShootProjectile();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void SummonMinions()
    {
        float summonRadius = 12f;
        float summonProbability = 0.3f;

        // If randomize hits, summon 2 minions
        if (Random.value < summonProbability)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector3 randomOffset = Random.insideUnitSphere * summonRadius;
                randomOffset.y = 0f;
                Vector3 potentialPosition = transform.position + randomOffset;

                RaycastHit hit;
                int layerMask = LayerMask.GetMask("WhatIsGround");

                // Perform a raycast downwards from a high point above the potential position
                if (Physics.Raycast(potentialPosition + Vector3.up * 50, Vector3.down, out hit, Mathf.Infinity, layerMask))
                {
                    Vector3 summonPosition = hit.point;

                    // Random minion prefab
                    int randomIndex = Random.Range(0, minionPrefab.Length);
                    GameObject pref = minionPrefab[randomIndex];
                    pref.GetComponent<ProtoMob>().player = player;
                    GameObject newMinion = Instantiate(pref, summonPosition, Quaternion.identity);
                }
            }

            // Reset the cooldown timer
            lastSummonTime = Time.time;
        }
    }


}

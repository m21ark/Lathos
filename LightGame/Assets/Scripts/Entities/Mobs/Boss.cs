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
    public float summonFrequency = 5.0f;
    [SerializeField] private GameObject[] minionPrefab;
    public List<Transform> predefinedTransforms = new List<Transform>();

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

    public int numberOfSummons = 50;
    private int indexOfPosition = 0;
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
        if (numberOfSummons <= 0) currentBossPhase = 3;

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
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (isJumping)
        {
            if (isLanding || Quaternion.Angle(secondChild.transform.rotation, Quaternion.Euler(phase3TargetRotation)) < 0.01f)
            {
                LerpToGroundPhase();
                // put the secondchild rotation to 36.753f, -4.393f, 0.866f and activate the first child ...another way to do this
                // secondChild.transform.rotation = Quaternion.Euler(36.753f, -4.393f, 0.866f);
                // firstChild.SetActive(true);
                // secondChild.SetActive(false);
                // isJumping = false;
                isLanding = true && isJumping;
            }
            else
            {
                LerpToFlightPhase();
            }
            FlapWings();
            return;
        }
        else
        {
            AttackPlayer();
        }

        if (distanceToPlayer < 12.0f)
        {
            // select a pos from the list indexOfPosition
            agent.SetDestination(predefinedTransforms[indexOfPosition % predefinedTransforms.Count].position);
            indexOfPosition++;
            isJumping = true;
        }
        else
        {
            secondChild.transform.LookAt(player);
            firstChild.transform.LookAt(player);
        }
    }

    private void Phase2Behavior()
    {

        if (Time.time - lastSummonTime >= summonFrequency && numberOfSummons > 0) SummonMinions();

        LerpToFlightPhase();
        agent.radius = 1.0f;

        disableBoxCollider();

        Vector3 targetPosition = player.position + new Vector3(5, 5, 0); // stupid but the only thing that worked
        agent.SetDestination(targetPosition);

        FlapWings();
        isJumping = true;
    }


    private void Phase3Behavior()
    {
        // phase 3 behavior
        if (isJumping)
        {
            LerpToGroundPhase();
            FlapWings();
            return;
        }
    

        agent.radius = 7.12f;

        FollowPlayer();

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        // if close to the player then increase the speed and acceleration
        if (distanceToPlayer < attackRange)
        {
            agent.speed = 13.0f;
            agent.acceleration = 60.0f;

            if (distanceToPlayer < 12.0f)
            {
                AttackPlayer2();

            }
        }
        else
        {
            agent.speed = 7.0f;
            agent.acceleration = 8.0f;
        }

        enabledBoxCollider();
    }

    private void AttackPlayer2()
    {
        if (!alreadyAttacked)
        {

            // take player damage
            ProtoClass playerHealth = player.GetComponent<ProtoClass>();
            playerHealth.TakeDamage(damage);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }


    private void disableBoxCollider()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }
    }

    private void enabledBoxCollider()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            boxCollider.enabled = true;
        }
    }

    private void LerpToFlightPhase()
    {
        if (firstChild != null && secondChild != null)
        {
            firstChild.SetActive(false);
            secondChild.SetActive(true);
        }

        Quaternion targetRotation = Quaternion.Euler(phase3TargetRotation);
        secondChild.transform.rotation = Quaternion.RotateTowards(secondChild.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        agent.baseOffset = Mathf.Lerp(agent.baseOffset, 15f, Time.deltaTime * upspeed);
    }

    private void LerpToGroundPhase()
    {
        Quaternion targetRotation = Quaternion.Euler(36.753f, -4.393f, 0.866f);
        secondChild.transform.rotation = Quaternion.RotateTowards(secondChild.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * 2.0f);

        agent.baseOffset = Mathf.Lerp(agent.baseOffset, -0.23f, Time.deltaTime * upspeed * 3.0f);
        if (Quaternion.Angle(targetRotation, secondChild.transform.rotation) < 0.01f && secondChild.transform.rotation.y > -4.393f && agent.baseOffset < 0.40f)
        {
            agent.baseOffset = -0.23f;
            if (firstChild != null && secondChild != null)
            {
                firstChild.SetActive(true);
                secondChild.SetActive(false);
            }
            // set rotation to  36.753f, -4.393f, 0.866f
            secondChild.transform.localRotation = Quaternion.Euler(36.753f, -4.393f, 0.866f);

            // set local rotation to 0 0 0
            firstChild.transform.localRotation = Quaternion.Euler(0, 0, 0);

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

    public void FollowPlayer()
    {
        agent.SetDestination(player.position);
    }

    public override void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        if (!alreadyAttacked)
        {
            ShootProjectile();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void SummonMinions()
    {
        float summonRadius = 12f;
        float summonProbability = 0.3f;

        if (Random.value < summonProbability)
        {
            for (int i = 0; i < 4; i++)
            {
                numberOfSummons--;
                health -= 25; // cost of summoning minions ... TODO: create a var
                Vector3 randomOffset = Random.insideUnitSphere * summonRadius;
                randomOffset.y = 0f;
                Vector3 potentialPosition = transform.position + randomOffset;

                RaycastHit hit;
                int layerMask = LayerMask.GetMask("WhatIsGround");

                if (Physics.Raycast(potentialPosition + Vector3.up * 50, Vector3.down, out hit, Mathf.Infinity, layerMask))
                {
                    Vector3 summonPosition = hit.point;

                    int randomIndex = Random.Range(0, minionPrefab.Length);
                    GameObject pref = minionPrefab[randomIndex];
                    pref.GetComponent<ProtoMob>().player = player;
                    Instantiate(pref, summonPosition, Quaternion.identity);
                }
            }

            lastSummonTime = Time.time;
        }
    }
}

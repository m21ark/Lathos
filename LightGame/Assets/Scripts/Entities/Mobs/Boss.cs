using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : ProtoMob
{
    // Phase Logic
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


    private float rotationSpeed = 4.0f; // Speed of rotation
    private float upspeed = 0.5f;

    public float flapFrequency = 2f;
    public float flapAmplitude = 30f;

    private float flapTime;

    public Vector3 phase3TargetRotation = new Vector3(63.121f, 48.889f, -104.672f); // Target rotation in Euler angles


    void Start()
    {
        lastSummonTime = Time.time;

        firstChild = transform.GetChild(0).gameObject;
        secondChild = transform.GetChild(1).gameObject;

        // Ensure initial visibility state
        firstChild.SetActive(true);
        secondChild.SetActive(false);
    }

    void Update()
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

    private void Phase1Behavior()
    {
        // Move2();
        if (Time.time - lastSummonTime >= summonFrequency * 2) SummonMinions();
    }

    private void Phase2Behavior()
    {


        // Summoning phase and follow player
        Move2();

        // Summon minion if 10s cooldown has passed
        if (Time.time - lastSummonTime >= summonFrequency) SummonMinions();
    }

    private void Phase3Behavior()
    {
        // You should make visible the second child and make invisible the first child
        if (firstChild != null && secondChild != null)
        {
            firstChild.SetActive(false);
            secondChild.SetActive(true);
        }

        Quaternion targetRotation = Quaternion.Euler(phase3TargetRotation);
        secondChild.transform.rotation = Quaternion.RotateTowards(secondChild.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);




        // set agent offset to 10
        agent.baseOffset = Mathf.Lerp(agent.baseOffset, 15f, Time.deltaTime * upspeed);
        if (Time.time - lastSummonTime >= summonFrequency) SummonMinions();

        agent.SetDestination(player.position);

        FlapWings();

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

    void OnCollisionEnter(Collision collision)
    {
        // Deal Damage ... TODO: maybe change this to put like ant
        if (collision.gameObject.CompareTag("Player"))
            GameLogic.instance.player.TakeDamage(damage);
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
                Vector3 summonPosition = transform.position + randomOffset;

                // random minion prefab
                int randomIndex = Random.Range(0, minionPrefab.Length);
                GameObject pref = minionPrefab[randomIndex];
                pref.GetComponent<ProtoMob>().player = player;
                GameObject newMinion = Instantiate(pref, summonPosition, Quaternion.identity);

            }

            // Reset the cooldown timer
            lastSummonTime = Time.time;
        }
    }

}

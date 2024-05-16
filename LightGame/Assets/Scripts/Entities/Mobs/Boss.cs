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
    public GameObject minionPrefab;

    void Start()
    {
        lastSummonTime = Time.time;
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
        // Do nothing
    }

    private void Phase2Behavior()
    {
        ChangeColor(Color.blue);

        // Summoning phase and follow player
        Move2();

        // Summon minion if 10s cooldown has passed
        if (Time.time - lastSummonTime >= summonFrequency) SummonMinions();
    }

    private void Phase3Behavior()
    {
        // Do nothing
        ChangeColor(Color.green);
    }

    public void Move2()
    {
        // Calculate direction vector towards the player
        Vector3 direction = GameLogic.instance.player.gameObject.transform.position - transform.position;
        direction.y = 0f; // Ensure the minion doesn't move up or down

        // Normalize the direction vector
        direction.Normalize();

        // Move the minion towards the player
        transform.Translate(direction * 3 * Time.deltaTime);    // NOTE: THIS IS GOING TO Disappear
    }

    void OnCollisionEnter(Collision collision)
    {
        // Deal Damage
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
                GameObject newMinion = Instantiate(minionPrefab, summonPosition, Quaternion.identity);
            }

            // Reset the cooldown timer
            lastSummonTime = Time.time;
        }
    }

    private void ChangeColor(Color newColor)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        Material material = new Material(renderer.material);
        material.color = newColor;
        renderer.material = material;
    }
}

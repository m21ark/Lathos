using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionBehavior2 : ProtoMob
{
    GameObject player;

    private float lastSummonTime;
    public float summonFrequency = 10.0f;
    public int maxMinions = 10;

    void Update()
    {
        Move();
        if (GameLogic.instance.player)
            player = GameLogic.instance.player.getGameObject();

        // Summon minion if 10s cooldown has passed
        if (Time.time - lastSummonTime >= summonFrequency) SummonMinions();
    }

    public override void Move()
    {

        if (player != null)
        {
            // Calculate direction vector towards the player
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0f; // Ensure the minion doesn't move up or down

            // Normalize the direction vector
            direction.Normalize();

            // Move the minion towards the player
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {

        // Deal Damage
        if (collision.gameObject.CompareTag("Player"))
            GameLogic.instance.player.TakeDamage(damage);
    }

    void SummonMinions()
    {

        // Reset the cooldown timer
        lastSummonTime = Time.time;

        float summonRadius = 8f;
        float summonProbability = 0.3f;

        // If randomize hits, summon 2 minions
        if (Random.value < summonProbability)
        {
            for (int i = 0; i < 2; i++)
            {

                if (CountActiveMinions() >= maxMinions)
                    return;

                Vector3 randomOffset = Random.insideUnitSphere * summonRadius;
                randomOffset.y = 2f;
                Vector3 summonPosition = transform.position + randomOffset;
                GameObject newMinion = Instantiate(gameObject, summonPosition, Quaternion.identity);
            }
        }
    }

    int CountActiveMinions()
    {
        // Find all active instances of Minion class in the scene
        MinionBehavior2[] minions = FindObjectsOfType<MinionBehavior2>();
        return minions.Length;
    }
}

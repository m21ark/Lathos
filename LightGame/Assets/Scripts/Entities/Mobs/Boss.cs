using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : ProtoMob
{
    public GameObject minionPrefab;
    private GameLogic gameLogic;
    private float lastSummonTime;
    GameObject player;

    void Start()
    {
        gameLogic = GameObject.FindWithTag("GameController").GetComponent<GameLogic>();
        lastSummonTime = Time.time;
    }

    void Update()
    {
        Move();

        player = gameLogic.player.getGameObject();

        // Summon minion if 10s cooldown has passed
        if (Time.time - lastSummonTime >= 10.0f) SummonMinions();   
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
        if (collision.gameObject.CompareTag("Player")){
            gameLogic.player.TakeDamage(30);
            JumpAwayFromPlayer();
        }
      
    }

    void JumpAwayFromPlayer()
    {
        // Calculate direction away from the player
        Vector3 direction = transform.position - player.transform.position;
        direction.y = 0f; 
        direction.Normalize();

        float jumpForce = 1000.0f;
        GetComponent<Rigidbody>().AddForce(direction * jumpForce, ForceMode.Impulse);
    }

    void SummonMinions(){
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
}

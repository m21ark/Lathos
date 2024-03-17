using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHitEnemy : MonoBehaviour
{
    private GameLogic gameLogic;
    private float travelTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameLogic = GameObject.FindWithTag("GameController").GetComponent<GameLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        travelTime += Time.deltaTime;

        // If too long in the air, destroy projectile
        if(travelTime > 5.0f) Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Damage the boss
        if (collision.gameObject.CompareTag("Boss")){
            gameLogic.damageBoss(10);
            Destroy(gameObject);
        }
    }
}

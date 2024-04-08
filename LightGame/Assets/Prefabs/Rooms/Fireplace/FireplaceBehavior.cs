using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireplaceBehavior : MonoBehaviour
{

    public bool isLit = false;
    public int healAmount = 1;
    public float lightLimit = 20;
    public GameObject FireLightObj;
    private GameLogic gameLogic;

    void Start()
    {
        gameLogic = GameObject.FindWithTag("GameController").GetComponent<GameLogic>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isLit)
        {
            isLit = true;
            FireLightObj.SetActive(true);
            GetComponent<Renderer>().material.color = new Color(1f, 0.5f, 0f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ProtoClass player = other.GetComponent<ProtoClass>();

            player.Heal(healAmount);

            if(player.collectedLight < lightLimit)
                player.collectedLight += 1;
            
        }
    }
}

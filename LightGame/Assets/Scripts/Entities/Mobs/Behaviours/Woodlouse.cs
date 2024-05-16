using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Woodlouse : ProtoMob
{
    private Vector3 initialPosition;
    private bool backToPos;

    private void Start()
    {
        // Store the initial position
        initialPosition = transform.position;
    }

    public override void AttackPlayer()
    {
        // Set the destination to the player's position
        if (!backToPos) agent.SetDestination(player.position);
        else agent.SetDestination(initialPosition);

        // if initial position is close to the player's position backToPos then set the backToPos to false
        if (Vector3.Distance(initialPosition, player.position) < 1f) backToPos = false;

        if (!alreadyAttacked)
        {
            initialPosition = transform.position;

            // Attack code here
            attack();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public override void attack()
    {
        Debug.Log("Attacking Woodlouse");

        // Invoke the MoveBack method after a delay
        Invoke(nameof(MoveBack), timeBetweenAttacks - 0.2f);
    }

    public override void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void MoveBack()
    {
        backToPos = true;
        // Set the destination back to the initial position
        agent.SetDestination(initialPosition);
    }
}

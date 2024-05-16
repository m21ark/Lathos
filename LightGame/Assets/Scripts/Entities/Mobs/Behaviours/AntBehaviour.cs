using UnityEngine;

public class AntBehaviour : ProtoMob
{
    public Transform leader;

    private void Update()
    {
        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) FollowLeader();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    private void FollowLeader()
    {
        if (leader != null)
        {
            agent.SetDestination(leader.position);
        }
    }
}

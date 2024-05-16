using UnityEngine;

public class AntBehaviour : ProtoMob
{
    public Transform leader;

    public float angularView = 5.0f; 

    private void Update()
    {
        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange && !attackedByPlayer) FollowLeader();
        if ((playerInSightRange && !playerInAttackRange) || attackedByPlayer) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

        // Make the meshes stick to the ground

        //if does not have a first child then return immediately
        // if (transform.childCount == 0) return;

        // // Perform raycast from the object's position downwards
        // RaycastHit hit;
        // if (Physics.Raycast(transform.GetChild(0).position, Vector3.down, out hit, 10f))
        // {
        //     // Get the normal of the hit point
        //     Vector3 normal = hit.normal;

        //     // Calculate the rotation needed to be parallel to the ground
        //     Quaternion rotation = Quaternion.FromToRotation(transform.GetChild(0).up, normal) * transform.GetChild(0).rotation;

        //     // Apply the rotation
        //     transform.GetChild(0).rotation = Quaternion.Lerp(transform.GetChild(0).rotation, rotation, 0.1f);
        // }
    }

    private void FollowLeader()
    {
       
        Patrolling();
        
    }
   protected override void SearchWalkPoint()
    {
        if (leader != null)
        {
            // Generate a random angle within the specified range
            float randomAngle = Random.Range(-angularView, angularView);

            // Calculate the direction based on this random angle
            Quaternion rotation = Quaternion.Euler(0f, randomAngle, 0f);
            Vector3 direction = rotation * transform.forward;

            // Set the target position in front of the leader
            Vector3 targetPosition = leader.position + direction.normalized * 10f;

            // Ensure the target position is within 20 units of the initial position
            if (Vector3.Distance(initialPosition, targetPosition) <= 30f)
            {
                // Perform a raycast to check if the target position is on the ground
                if (Physics.Raycast(targetPosition, Vector3.down, out RaycastHit hit, 12f, whatIsGround))
                {
                    walkPoint = hit.point; // Set the walk point to the hit point on the ground
                    walkPointSet = true;   // Mark that a valid walk point has been set
                }
                else
                {
                    walkPointSet = false;  // No valid ground found, so no walk point set
                }
            }
            else
            {
                walkPointSet = false; // Target position is too far, so no walk point set
            }
        }
        else
        {
            // Generate a random angle within the specified range
            float randomAngle = Random.Range(-angularView, angularView);

            // Calculate the direction based on this random angle
            Quaternion rotation = Quaternion.Euler(0f, randomAngle, 0f);
            Vector3 direction = rotation * transform.forward;

            // Set the target position 5 units in front of the ant
            Vector3 targetPosition = transform.position + direction.normalized * 5f;

            // Ensure the target position is within 20 units of the initial position
            if (Vector3.Distance(initialPosition, targetPosition) <= 30f)
            {
                // Perform a raycast to check if the target position is on the ground
                if (Physics.Raycast(targetPosition, Vector3.down, out RaycastHit hit, 12f, whatIsGround))
                {
                    walkPoint = hit.point; // Set the walk point to the hit point on the ground
                    walkPointSet = true;   // Mark that a valid walk point has been set
                    angularView = 5.0f;
                }
                else
                {
                    walkPointSet = false;  // No valid ground found, so no walk point set
                }
            }
            else
            {
                angularView += 5.0f;
                walkPointSet = false; // Target position is too far, so no walk point set
            }
        }
    }



}

using UnityEngine;

public class LampHoverPlayer : MonoBehaviour
{
    public Transform player;
    public float hoverRadius = 3f; // Radius within which the lamp hovers around the player
    public float hoverHeight = 2f; // Height above the player the lamp hovers
    public float baseMovementSpeed = 200f; // Base speed of movement towards the target position
    public float maxSpeedDistance = 200f; // Distance at which the firefly moves at maximum speed
    public float smoothness = 3f; // Smoothness of movement
    public float cancelDistance = 5f; // Distance threshold to cancel current motion and set new target position
    public LayerMask orbLayer; // Layer mask for orbs

    private Vector3 targetPosition;
    private Vector3 currentVelocity;
    private bool seekingOrb = false;
    private GameObject nearestOrb;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;

            if (player == null)
            {
                Debug.LogError("Player GameObject not found with the 'Player' tag!");
                enabled = false; // Disable the script if player reference is not found
                return;
            }
        }

        // Set initial target position
        SetRandomTargetPosition();
    }

    void Update()
    {
        // Decide target
        if (!seekingOrb) HoverAroundPlayer();
        else SeekOrb();

        // Move towards target
        MoveTowardsTarget();
    }

    void HoverAroundPlayer()
    {
        // Check if the player is farther than the cancel distance
        if (Vector3.Distance(transform.position, player.position) > cancelDistance)
        {
            // Cancel the current motion and set a new random target position near the player
            SetRandomTargetPosition();
        }

        // Check if there are orbs around
        LookForOrbs();
    }

    void LookForOrbs()
    {
        // Look for nearby orbs with the tag "OrbXP"
        Collider[] orbs = Physics.OverlapSphere(transform.position, 10);
        float nearestDistance = float.MaxValue; // Declare nearestDistance here
        foreach (Collider orb in orbs)
        {
            if (orb.CompareTag("OrbXP"))
            {
                float distance = Vector3.Distance(transform.position, orb.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestOrb = orb.gameObject;
                }
            }
        }

        // If a valid orb is found, set the target position to the nearest orb
        if (nearestOrb != null)
        {
            targetPosition = nearestOrb.transform.position;
            seekingOrb = true;
        }
    }

    void SeekOrb()
    {
        if (nearestOrb == null)
        {
            // If there's no nearest orb, stop seeking and resume hovering around the player
            seekingOrb = false;
            SetRandomTargetPosition();
            return;
        }

        // Check if the nearest orb is too far away
        float distanceToOrb = Vector3.Distance(transform.position, nearestOrb.transform.position);
        if (distanceToOrb > 20f)
        {
            // If the nearest orb is too far away, stop seeking and resume hovering around the player
            seekingOrb = false;
            SetRandomTargetPosition();
            return;
        }

        // Set the target position to the nearest orb
        targetPosition = nearestOrb.transform.position;
    }


    void MoveTowardsTarget()
    {
        // Calculate direction and distance to the target position
        Vector3 targetDirection = (targetPosition - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        // Calculate movement speed based on distance from the player
        float scaledMovementSpeed = baseMovementSpeed * Mathf.Clamp01(distanceToTarget / maxSpeedDistance) * 2f;

        // Calculate smoothed movement speed
        float smoothedMovementSpeed = Mathf.Lerp(0, scaledMovementSpeed, smoothness * Time.deltaTime);

        // Move towards the target position using Lerp for smooth motion
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothedMovementSpeed);

        // If the firefly is close to the target position, set a new random target position
        if (distanceToTarget < 1f && !seekingOrb)
        {
            SetRandomTargetPosition();
        }
    }

    // Sets a random target position around the player
    void SetRandomTargetPosition()
    {
        if (!seekingOrb)
        {
            Vector3 randomOffset = Random.insideUnitSphere * hoverRadius;
            randomOffset.y = Mathf.Abs(randomOffset.y); // Ensure lamp doesn't hover underground
            targetPosition = player.position + randomOffset + Vector3.up * hoverHeight;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OrbXP"))
        {
            Destroy(other.gameObject);
            seekingOrb = false;
            SetRandomTargetPosition();
        }
    }
}

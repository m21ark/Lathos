using UnityEngine;

public class LampHoverPlayer : MonoBehaviour
{
    public Transform player;
    public float hoverRadius = 3f; // Radius within which the lamp hovers around the player
    public float hoverHeight = 2f; // Height above the player the lamp hovers
    public float hoverSpeed = 1f; // Speed of hovering
    public float returnSpeed = 20f; // Speed of returning to the player if too far

    private Vector3 targetPosition;

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
        SetPlayerTargetPosition();
    }

    void Update()
    {
        // Move towards target
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        // If the firefly is close to the player, set the target position near the player
        if (Vector3.Distance(transform.position, player.position) > hoverRadius)
        {
            SetPlayerTargetPosition();
        }

        // Calculate direction and distance to the target position
        Vector3 targetDirection = (targetPosition - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        // Move towards the target position smoothly
        transform.position = Vector3.Lerp(transform.position, targetPosition, hoverSpeed * Time.deltaTime);

        // If the firefly is close to the target position, set a new random target position
        if (distanceToTarget < 0.1f)
        {
            SetPlayerTargetPosition();
        }
    }

    // Sets the target position near the player
    void SetPlayerTargetPosition()
    {
        Vector2 randomOffset = Random.insideUnitCircle * hoverRadius;
        targetPosition = player.position + new Vector3(randomOffset.x, hoverHeight, randomOffset.y);
    }
}

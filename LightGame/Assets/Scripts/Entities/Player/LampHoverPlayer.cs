using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LampHoverPlayer : MonoBehaviour
{
    // Movement variables
    private float hoverRadius = 0.8f; 
    private float hoverHeight = 2f;
    private float hoverSpeed = 2f;
    private float followForce = 45f; 
    private float catchUpForce = 180f;
    private float maxDistance = 10f;
    private float damping = 5f;

    public GameObject lightSource;

    private Vector3 targetPosition;
    private Vector3 hoverTargetPosition;
    private Transform player;
    private int playerLight = 0;
    private Rigidbody rb;
    private Transform cameraPivot;
    private bool hasReachedHoverTarget = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        GetPlayer();
    }

    void GetPlayer()
    {
        player = GameLogic.instance.player.transform;
        cameraPivot = player.parent.transform.Find("CameraPivot").transform;
    }

    void UpdateLightSource()
    {
        playerLight = player.GetComponent<ProtoClass>().collectedLight;
        Light light = lightSource.GetComponent<Light>();
        light.intensity = playerLight * 0.04f;
    }

    void FixedUpdate()
    {
        UpdateLightSource();
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        float cameraPivotY = cameraPivot.rotation.eulerAngles.y;

        Vector3 leftOfPlayer = player.position - player.right;
        targetPosition = leftOfPlayer + new Vector3(0, hoverHeight, 0);
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance < hoverRadius)
        {
            // Lamp is near player, so it hovers slightly
            Hover();
        }
        else if (distance < maxDistance)
        {
            // Player is within a reasonable distance, lamp follows at normal speed
            ApplyForceTowardsTarget(followForce, targetPosition);
        }
        else
        {
            // Player is far, lamp catches up quickly
            ApplyForceTowardsTarget(catchUpForce, targetPosition);
        }

        // Apply damping to reduce oscillation
        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, damping * Time.fixedDeltaTime);
    }

    void Hover()
    {
        if (hasReachedHoverTarget)
        {
            Vector2 randomOffset = Random.insideUnitCircle * hoverRadius;
            hoverTargetPosition = targetPosition + new Vector3(randomOffset.x, 0, randomOffset.y);
            hasReachedHoverTarget = false;
        }

        ApplyForceTowardsTarget(hoverSpeed, hoverTargetPosition);

        // Check if the lamp has reached the hover target
        if (Vector3.Distance(transform.position, hoverTargetPosition) < 0.1f)
            hasReachedHoverTarget = true;
    }

    void ApplyForceTowardsTarget(float force, Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        rb.AddForce(direction * force);
    }
}

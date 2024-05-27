using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntProcedural : MonoBehaviour
{
    [SerializeField] private Transform[] legTargetTransforms;

    public float stepDistance = 2f;
    public float raycastRange = 4f;
    public float rayCastHeight = 2f;
    public float rayOffset = 0.5f;
    public float bodyHeight = 1.0f;
    public float startPosVariation = 0.5f;
    public float walkDemoSpeed = 0;

    protected Vector3[] currentLegPositions;
    protected Vector3[] rayCasterPositions;
    protected Vector3[] startLegPositions;
    protected Vector3[] raycastHitNormals;
    protected bool[] isLegMoving;
    protected int numLegs;

    private void Start()
    {
        numLegs = legTargetTransforms.Length;

        currentLegPositions = new Vector3[numLegs];
        rayCasterPositions = new Vector3[numLegs];
        startLegPositions = new Vector3[numLegs];
        raycastHitNormals = new Vector3[numLegs];

        isLegMoving = new bool[numLegs];

        for (int i = 0; i < numLegs; i++)
            isLegMoving[i] = false;

        for (int i = 0; i < numLegs; i++)
            startLegPositions[i] = legTargetTransforms[i].localPosition;

        SetRayCastPositions();

        // give some initial offset to the legs random
        for (int i = 0; i < numLegs; i++)
            legTargetTransforms[i].position += Random.Range(-startPosVariation, startPosVariation) * transform.forward;

        for (int i = 0; i < numLegs; i++)
            currentLegPositions[i] = legTargetTransforms[i].position;
    }

    private void SetRayCastPositions()
    {
        for (int i = 0; i < numLegs; i++)
        {
            Vector3 relativeTransformPosition = transform.TransformPoint(startLegPositions[i]) - transform.position;
            rayCasterPositions[i] = relativeTransformPosition + rayOffset * transform.forward + rayCastHeight * transform.up;
        }
    }

    private void Update()
    {
        // Check all legs
        for (int i = 0; i < 6; i++)
            CheckLeg(legTargetTransforms[i], i);

        // Rotate the body to align with the ground
        InclineBody();

        SetRayCastPositions();

        if (walkDemoSpeed > 0)
            transform.position += transform.forward * Time.deltaTime * walkDemoSpeed;
    }

    // private void InclineBody()
    // {
    //     Vector3 averageNormal = Vector3.zero;
    //     float averageDistanceToGround = 0f;

    //     for (int i = 0; i < numLegs; i++)
    //     {
    //         averageNormal += raycastHitNormals[i];
    //         averageDistanceToGround += Mathf.Abs(currentLegPositions[i].y - transform.position.y);
    //     }

    //     averageNormal /= numLegs;
    //     averageDistanceToGround /= numLegs;
    //     averageNormal.Normalize();

    //     Vector3 targetPosition = transform.position + transform.up * (bodyHeight - averageDistanceToGround);
    //     transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 2f);

    //     Quaternion targetRotation = Quaternion.FromToRotation(transform.up, averageNormal) * transform.rotation;
    //     transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
    // }

    public virtual void InclineBody()
    {
        Vector3 averageNormal = Vector3.zero;
        float averageDistanceToGround = 0f;

        for (int i = 0; i < numLegs; i++)
        {
            averageNormal += raycastHitNormals[i];
            averageDistanceToGround += Mathf.Abs(currentLegPositions[i].y - transform.position.y);
        }

        averageNormal /= numLegs;
        averageDistanceToGround /= numLegs;
        averageNormal.Normalize();

        // Calculate the target position relative to the parent
        Vector3 targetPosition = transform.position + transform.up * (bodyHeight - averageDistanceToGround);

        // Maintain a relative distance to the parent
        Vector3 parentPosition = transform.parent.position;
        Vector3 offsetFromParent = targetPosition - parentPosition;

        // Constrain the y-coordinate between -2.6 and -1.4 units relative to the parent
        float minY = parentPosition.y - 2.6f;
        float maxY = parentPosition.y - 1.4f;
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);

        // Constrain the x and z coordinates to not differ more than 2.1 units from the parent
        float maxDistanceXZ = 1.1f;
        float distanceX = Mathf.Clamp(targetPosition.x - parentPosition.x, -maxDistanceXZ, maxDistanceXZ);
        float distanceZ = Mathf.Clamp(targetPosition.z - parentPosition.z, -maxDistanceXZ, maxDistanceXZ);

        targetPosition.x = parentPosition.x + distanceX;
        targetPosition.z = parentPosition.z + distanceZ;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 2f);

        // Calculate the target rotation without changing the y-axis rotation
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, averageNormal) * transform.rotation;
        targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, transform.rotation.eulerAngles.y, targetRotation.eulerAngles.z);

        // Apply the rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
    }


    private void CheckLeg(Transform leg, int index)
    {
        Vector3 origin = rayCasterPositions[index] + transform.position;
        Ray ray = new Ray(origin, -transform.up);

        Debug.DrawRay(ray.origin, ray.direction * raycastRange, Color.blue, 0.02f);

        if (Physics.Raycast(ray, out RaycastHit hit, raycastRange))
        {
            if (Vector3.Distance(hit.point, currentLegPositions[index]) > stepDistance && !CheckIfAnyLegMoving())
            {
                currentLegPositions[index] = hit.point;
                raycastHitNormals[index] = hit.normal;
                isLegMoving[index] = true;
            }
        }
        else currentLegPositions[index] = startLegPositions[index] + transform.position;

        StartCoroutine(MoveLegCoroutine(leg, index));
    }

    private IEnumerator MoveLegCoroutine(Transform leg, int index)
    {
        leg.position = Vector3.Lerp(leg.position, currentLegPositions[index], Time.deltaTime * 25f * (walkDemoSpeed > 0 ? walkDemoSpeed : 1));
        if (Vector3.Distance(leg.position, currentLegPositions[index]) < 0.1f) isLegMoving[index] = false;
        yield return null;
    }

    private bool CheckIfAnyLegMoving()
    {
        for (int i = 0; i < 6; i++)
            if (isLegMoving[i]) return true;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < numLegs; i++)
            Gizmos.DrawWireSphere(legTargetTransforms[i].position, 0.2f);
    }
}

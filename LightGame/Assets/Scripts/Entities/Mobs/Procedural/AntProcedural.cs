using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntProcedural : MonoBehaviour
{
    [SerializeField] private Transform leftBackLegTarget;
    [SerializeField] private Transform rightBackLegTarget;
    [SerializeField] private Transform leftFrontLegTarget;
    [SerializeField] private Transform rightFrontLegTarget;
    [SerializeField] private Transform rightMidLegTarget;
    [SerializeField] private Transform leftMidLegTarget;

    public float footDistanceToBody = 1.5f;
    public float stepDistance = 2f;
    public float startOffset = 0.5f;

    public int walkDemoSpeed = 0;

    private float raycastRange = 3f;

    private Vector3[] newLegPositions = new Vector3[6];
    private Vector3[] currentLegPositions = new Vector3[6];
    private Vector3[] legStartPositions = new Vector3[6];

    private void Start()
    {
        newLegPositions[0] = leftBackLegTarget.position;
        newLegPositions[1] = rightBackLegTarget.position;
        newLegPositions[2] = leftFrontLegTarget.position;
        newLegPositions[3] = rightFrontLegTarget.position;
        newLegPositions[4] = rightMidLegTarget.position;
        newLegPositions[5] = leftMidLegTarget.position;

        currentLegPositions[0] = leftBackLegTarget.position;
        currentLegPositions[1] = rightBackLegTarget.position;
        currentLegPositions[2] = leftFrontLegTarget.position;
        currentLegPositions[3] = rightFrontLegTarget.position;
        currentLegPositions[4] = rightMidLegTarget.position;
        currentLegPositions[5] = leftMidLegTarget.position;

        legStartPositions[0] = leftBackLegTarget.position - (startOffset + 0.7f) * Vector3.forward;
        legStartPositions[1] = rightBackLegTarget.position - (startOffset + 0.5f) * Vector3.forward;
        legStartPositions[2] = leftFrontLegTarget.position - (startOffset - 1f) * Vector3.forward;
        legStartPositions[3] = rightFrontLegTarget.position - (startOffset + 0.8f) * Vector3.forward;
        legStartPositions[4] = rightMidLegTarget.position - (startOffset - 0.3f) * Vector3.forward;
        legStartPositions[5] = leftMidLegTarget.position - (startOffset + 0.4f) * Vector3.forward;
    }

    void Update()
    {
        CheckLeg(leftBackLegTarget, 0);
        CheckLeg(rightMidLegTarget, 4);
        CheckLeg(leftFrontLegTarget, 2);

        CheckLeg(rightBackLegTarget, 1);
        CheckLeg(rightFrontLegTarget, 3);
        CheckLeg(leftMidLegTarget, 5);

        if(walkDemoSpeed > 0)
            transform.position -= transform.forward * Time.deltaTime * walkDemoSpeed;
    }

    void CheckLeg(Transform leg, int index)
    {
        leg.position = currentLegPositions[index];
        Vector3 origin = legStartPositions[index] + transform.position;
        Ray ray = new Ray(origin, Vector3.down);

        // Debug.DrawRay(ray.origin, ray.direction * raycastRange, Color.blue, 0.5f);

        if (Physics.Raycast(ray, out RaycastHit hit, raycastRange))
            if (Vector3.Distance(hit.point, currentLegPositions[index]) > stepDistance)
                newLegPositions[index] = hit.point;
   
        currentLegPositions[index] = Vector3.Lerp(currentLegPositions[index], newLegPositions[index], 0.1f);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(leftBackLegTarget.position, 0.2f);
        Gizmos.DrawWireSphere(rightBackLegTarget.position, 0.2f);
        Gizmos.DrawWireSphere(leftFrontLegTarget.position, 0.2f);
        Gizmos.DrawWireSphere(rightFrontLegTarget.position, 0.2f);
        Gizmos.DrawWireSphere(rightMidLegTarget.position, 0.2f);
        Gizmos.DrawWireSphere(leftMidLegTarget.position, 0.2f);

        Gizmos.color = Color.red;
        if(legStartPositions.Length > 0)
        {
            Gizmos.DrawWireSphere(legStartPositions[0] + transform.position, 0.2f);
            Gizmos.DrawWireSphere(legStartPositions[1] + transform.position, 0.2f);
            Gizmos.DrawWireSphere(legStartPositions[2] + transform.position, 0.2f);
            Gizmos.DrawWireSphere(legStartPositions[3] + transform.position, 0.2f);
            Gizmos.DrawWireSphere(legStartPositions[4] + transform.position, 0.2f);
            Gizmos.DrawWireSphere(legStartPositions[5] + transform.position, 0.2f);
        }
    }
}

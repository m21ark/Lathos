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

    public float stepDistance = 2f;
    
    public float raycastRange = 4f;
    public float rayCastHeight = 2f;

    public float rayOffset = 0.5f;

    public float startPosVariation = 0.5f;

    public int walkDemoSpeed = 0;

    private Vector3[] currentLegPositions = new Vector3[6];
    private Vector3[] rayCasterPositions = new Vector3[6];
    private Vector3[] startLegPositions = new Vector3[6];
    private bool[] isLegMoving = new bool[6];

    private void Start()
    {
        for(int i = 0; i < 6; i++)
            isLegMoving[i] = false;

        startLegPositions = new Vector3[]
        {
            leftBackLegTarget.localPosition,
            rightBackLegTarget.localPosition,
            leftFrontLegTarget.localPosition,
            rightFrontLegTarget.localPosition,
            rightMidLegTarget.localPosition,
            leftMidLegTarget.localPosition
        };

        for(int i = 0; i < 6; i++) 
            rayCasterPositions[i] = startLegPositions[i] + rayOffset * Vector3.forward + rayCastHeight * Vector3.up; 

        // give some initial offset to the legs random
        leftBackLegTarget.position += Random.Range(-startPosVariation, startPosVariation) * Vector3.forward;
        rightBackLegTarget.position += Random.Range(-startPosVariation, startPosVariation) * Vector3.forward;
        leftFrontLegTarget.position += Random.Range(-startPosVariation, startPosVariation) * Vector3.forward;
        rightFrontLegTarget.position += Random.Range(-startPosVariation, startPosVariation) * Vector3.forward;
        rightMidLegTarget.position += Random.Range(-startPosVariation, startPosVariation) * Vector3.forward;
        leftMidLegTarget.position += Random.Range(-startPosVariation, startPosVariation) * Vector3.forward;

        currentLegPositions = new Vector3[]
        {
            leftBackLegTarget.position,
            rightBackLegTarget.position,
            leftFrontLegTarget.position,
            rightFrontLegTarget.position,
            rightMidLegTarget.position,
            leftMidLegTarget.position
        };
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
        Vector3 origin = rayCasterPositions[index] + transform.position;
        Ray ray = new Ray(origin, Vector3.down);

        Debug.DrawRay(ray.origin, ray.direction * raycastRange, Color.blue, 0.02f);

        if (Physics.Raycast(ray, out RaycastHit hit, raycastRange)){
            if (Vector3.Distance(hit.point, currentLegPositions[index]) > stepDistance && !CheckIfAnyLegMoving()){
                currentLegPositions[index] = hit.point;
                isLegMoving[index] = true;
            }
        } else currentLegPositions[index] = startLegPositions[index] + transform.position; 
            
        StartCoroutine(MoveLegCoroutine(leg, index));
    }

    IEnumerator MoveLegCoroutine(Transform leg, int index)
    {
        leg.position = Vector3.Lerp(leg.position, currentLegPositions[index], Time.deltaTime * 10f * walkDemoSpeed);
        if(Vector3.Distance(leg.position, currentLegPositions[index]) < 0.1f) isLegMoving[index] = false;
        yield return null;
    }

    bool CheckIfAnyLegMoving(){
        for(int i = 0; i < 6; i++)
            if(isLegMoving[i]) return true;
        return false;
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
        if(rayCasterPositions.Length > 0)
        {
            Gizmos.DrawWireSphere(rayCasterPositions[0] + transform.position, 0.2f);
            Gizmos.DrawWireSphere(rayCasterPositions[1] + transform.position, 0.2f);
            Gizmos.DrawWireSphere(rayCasterPositions[2] + transform.position, 0.2f);
            Gizmos.DrawWireSphere(rayCasterPositions[3] + transform.position, 0.2f);
            Gizmos.DrawWireSphere(rayCasterPositions[4] + transform.position, 0.2f);
            Gizmos.DrawWireSphere(rayCasterPositions[5] + transform.position, 0.2f);
        }
    }
}

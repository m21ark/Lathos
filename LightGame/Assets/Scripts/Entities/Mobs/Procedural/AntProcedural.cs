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

    private float startZ_leftBackLegTarget;
    private float startZ_rightBackLegTarget;
    private float startZ_leftFrontLegTarget;
    private float startZ_rightFrontLegTarget;
    private float startZ_rightMidLegTarget;
    private float startZ_leftMidLegTarget;

    public float legSpeed = 5f;
    public float moveDistance = 0.07f;
    public float positiveOffset = 0.5f;
    private float phaseOffset = Mathf.PI; // legs move in opposite directions

    private ProtoMob mob = null;

    private void Start()
    {
        startZ_leftBackLegTarget = leftBackLegTarget.localPosition.z;
        startZ_rightBackLegTarget = rightBackLegTarget.localPosition.z;
        startZ_leftFrontLegTarget = leftFrontLegTarget.localPosition.z;
        startZ_rightFrontLegTarget = rightFrontLegTarget.localPosition.z;
        startZ_rightMidLegTarget = rightMidLegTarget.localPosition.z;
        startZ_leftMidLegTarget = leftMidLegTarget.localPosition.z;

         mob = GetComponent<ProtoMob>();
    }

    private void Update()
    {
        if(mob.moveSpeed > 0)
            LegMovement();
    }

    private void LegMovement(){
        MoveLeg(startZ_leftBackLegTarget, leftBackLegTarget, 0);
        MoveLeg(startZ_rightMidLegTarget, rightMidLegTarget, 0);
        MoveLeg(startZ_leftFrontLegTarget, leftFrontLegTarget, 0);

        MoveLeg(startZ_rightBackLegTarget, rightBackLegTarget, 1);
        MoveLeg(startZ_leftMidLegTarget, leftMidLegTarget, 1);
        MoveLeg(startZ_rightFrontLegTarget, rightFrontLegTarget, 1); 
    }

    private void MoveLeg(float startZ, Transform leg, int legIndex)
    {
        // take the current position of the leg and add the move distance to it in cycles
        float delta = moveDistance * Mathf.Sin(Time.time * legSpeed * (1 + mob.moveSpeed) + phaseOffset * legIndex);
        leg.localPosition = new Vector3(leg.localPosition.x, leg.localPosition.y, startZ + delta + positiveOffset);
    }
}

// https://www.google.com/url?sa=i&url=https%3A%2F%2Fgenent.cals.ncsu.edu%2Fbug-bytes%2Fthorax%2Flocomotion%2F&psig=AOvVaw1y7tbJf8mUqpFI7hk1Kr5U&ust=1715547306540000&source=images&cd=vfe&opi=89978449&ved=0CBEQjRxqFwoTCMD8gPO9hoYDFQAAAAAdAAAAABAE
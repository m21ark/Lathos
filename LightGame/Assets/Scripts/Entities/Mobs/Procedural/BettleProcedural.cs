using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BettleProcedural : AntProcedural
{
    public override void InclineBody()
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

        Vector3 targetPosition = transform.position + transform.up * (bodyHeight - averageDistanceToGround);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 2f);

        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, averageNormal) * transform.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
    }

}

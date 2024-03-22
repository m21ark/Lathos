using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeOffset : MonoBehaviour
{
    public Vector3 rotationRange = new Vector3(0, 30, 0); // Range of rotation in each axis
    public Vector3 scaleRange = new Vector3(0.2f, 0.2f, 0.2f); // Range of scale
    public Vector3 positionRange = new Vector3(4, 0, 4); // Range of position

    // Start is called before the first frame update
    void Start()
    {
        // Generate random rotation
        Vector3 randomRotation = new Vector3(Random.Range(-rotationRange.x, rotationRange.x),
                                             Random.Range(-rotationRange.y, rotationRange.y),
                                             Random.Range(-rotationRange.z, rotationRange.z));

        // Generate random scale
        Vector3 randomScale = new Vector3(Random.Range(1 - scaleRange.x, 1 + scaleRange.x),
                                          Random.Range(1 - scaleRange.y, 1 + scaleRange.y),
                                          Random.Range(1 - scaleRange.z, 1 + scaleRange.z));

        // Generate random position
        Vector3 randomPosition = new Vector3(transform.position.x + Random.Range(-positionRange.x, positionRange.x),
                                             transform.position.y + Random.Range(-positionRange.y, positionRange.y),
                                             transform.position.z + Random.Range(-positionRange.z, positionRange.z));

        // Apply random transformations
        transform.rotation = Quaternion.Euler(randomRotation);
        transform.localScale = randomScale;
        transform.position = randomPosition;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRayLogic : MonoBehaviour
{
    [SerializeField] private float xRayDuration;

    // Get all the enemies with the tag Boss or Minion in the scene, and change their layer to RenderAbove
    void Start()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Boss");
        foreach (GameObject enemy in enemies)
        {
            enemy.layer = LayerMask.NameToLayer("RenderAbove");
            ChangeLayerAllChildren(enemy.transform, "RenderAbove");
        }

        enemies = GameObject.FindGameObjectsWithTag("Minion");
        foreach (GameObject enemy in enemies)
        {
            enemy.layer = LayerMask.NameToLayer("RenderAbove");
            ChangeLayerAllChildren(enemy.transform, "RenderAbove");
        }

        // Wait for xRayDuration seconds, then change the layer of all the enemies back to Default
        StartCoroutine(ResetLayer());
    }
    
    IEnumerator ResetLayer()
    {
        yield return new WaitForSeconds(xRayDuration);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Boss");
        foreach (GameObject enemy in enemies)
        {
            enemy.layer = LayerMask.NameToLayer("Default");
            ChangeLayerAllChildren(enemy.transform, "Default");
        }

        enemies = GameObject.FindGameObjectsWithTag("Minion");
        foreach (GameObject enemy in enemies)
        {
            enemy.layer = LayerMask.NameToLayer("Default");
            ChangeLayerAllChildren(enemy.transform, "Default");
        }
    }

    // Make sure to change the children of the enemies to RenderAbove as well
    public void ChangeLayerAllChildren(Transform parent, string layerName)
    {
        foreach (Transform child in parent)
        {
            child.gameObject.layer = LayerMask.NameToLayer(layerName);
            ChangeLayerAllChildren(child, layerName);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class XRayLogic : MonoBehaviour
{
    public float xRayDuration;
    [SerializeField] private ScriptableRendererFeature fullScreenGray;
    [SerializeField] private Material _material; // _GrayIntensity proerty used in the shader

    // Curve used to fade the x-ray effect in and out
    [SerializeField] private AnimationCurve fadeCurve;
    private float _timer;
    private enum XRayState { 
        Inactive,
        FadingIn,
        XRay,
        FadingOut
    };
    private XRayState _state = XRayState.Inactive;

    // Get all the enemies with the tag Boss or Minion in the scene, and change their layer to RenderAbove
    void Start()
    {   
        _timer = 0.0f;
        _state = XRayState.FadingIn;
        _material.SetFloat("_GrayIntensity", 0.0f);
        // Now that the enemies are x-rayed, we need to change the view to grayscale
        fullScreenGray.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (_state == XRayState.Inactive)
            return;
        
        // Update the material according to the fade curve
        float greyIntensity = fadeCurve.Evaluate(_timer / xRayDuration);
        _material.SetFloat("_GrayIntensity", greyIntensity);
        _timer += Time.deltaTime;

        if (_state == XRayState.FadingIn && greyIntensity >= 1.0f) {
            _state = XRayState.XRay;
            ChangeEnemiesLayer("RenderAbove");

        } else if (_state == XRayState.XRay && greyIntensity < 1.0f) {
            _state = XRayState.FadingOut;
            ChangeEnemiesLayer("Default");

        } else if (_state == XRayState.FadingOut && greyIntensity <= 0.0f) {
            _state = XRayState.Inactive;
            fullScreenGray.SetActive(false);
            Destroy(gameObject);
        }
    }



    private void ChangeEnemiesLayer(string layerName)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Boss");
        foreach (GameObject enemy in enemies)
        {
            enemy.layer = LayerMask.NameToLayer(layerName);
            ChangeLayerAllChildren(enemy.transform, layerName);
        }

        enemies = GameObject.FindGameObjectsWithTag("Minion");
        foreach (GameObject enemy in enemies)
        {
            enemy.layer = LayerMask.NameToLayer(layerName);
            ChangeLayerAllChildren(enemy.transform, layerName);
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

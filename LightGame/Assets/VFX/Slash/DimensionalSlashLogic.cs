using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class DimensionalSlashScreenLogic : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private float slashDuration;
    [SerializeField] private ScriptableRendererFeature fullScreenInverted;
    [SerializeField] private Material _material;
    [SerializeField] private AnimationCurve offesetCurve;
    [SerializeField] private bool isHorizontal;
    private float _timer;
    // Start is called before the first frame update
    void Start()
    {
        _timer = 0.0f;
        _material.SetFloat("_Offset", 0.0f);
        _material.SetFloat("_IsHorizontal", isHorizontal ? 1.0f : 0.0f);
        fullScreenInverted.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer < delay) {
            _timer += Time.deltaTime;
            return;
        }

        // Update the material according to the fade curve
        float offset = offesetCurve.Evaluate((_timer-delay) / slashDuration);
        _material.SetFloat("_Offset", offset);
        _timer += Time.deltaTime;

        if (_timer - delay >= slashDuration) {
            fullScreenInverted.SetActive(false);
            Destroy(gameObject);
        }
    }
}

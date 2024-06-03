using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using UnityEngine.VFX;

public class BerserkLogic : MonoBehaviour
{
    [SerializeField] private Material berserkMaterial;
    [SerializeField] private AnimationCurve berserkValueCurve;
    [SerializeField] private ScriptableRendererFeature fullScreenBerserk;
    [SerializeField] private Material fullScreenBerserkMaterial;
    [SerializeField] private VisualEffect vfx;
    public float berserkDuration;
    private float _timer = 0.0f;
    private bool _isBerserk = false;

    public void Initialize()
    {
        _isBerserk = true;
        _timer = 0.0f;
        fullScreenBerserkMaterial.SetFloat("_Intensity", 0.0f);
        fullScreenBerserk.SetActive(true);
        vfx.Play();
    }

    void CleanUp()
    {
        _isBerserk = false;
        fullScreenBerserkMaterial.SetFloat("_Intensity", 0.0f);
        fullScreenBerserk.SetActive(false);
        vfx.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isBerserk) { return; }

        float value = berserkValueCurve.Evaluate(_timer / berserkDuration);
        // We need to access the property FresnelColor of the shader, and change just the value/lightness of the color (HSV)
        // Let's keep the saturation at 100%, the hue at 0, and the emission intensity at 3.5
        Color color = Color.HSVToRGB(0, 1, value);
        berserkMaterial.SetColor("_FresnelColor", color);

        // We need to access the property _Intensity of the shader, and change it according to the curve
        fullScreenBerserkMaterial.SetFloat("_Intensity", value);

        _timer += Time.deltaTime;

        // If the timer is greater than the duration, we should stop the berserk mode
        if (_timer >= berserkDuration)
        {
            CleanUp();
        }
    }

    private void OnDestroy() {
        CleanUp();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class DimensionalSlashScreenLogic : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private float slashDuration;
    [SerializeField] private float afterEffectDuration;
    [SerializeField] private ScriptableRendererFeature fullScreenInverted;

    private void OnEnable() {
        // Start the coroutine to change the view to inverted colors
        StartCoroutine(ChangeColors());
    }

    IEnumerator ChangeColors() {
        yield return new WaitForSeconds(delay);
        fullScreenInverted.SetActive(true);
        yield return new WaitForSeconds(slashDuration);
        fullScreenInverted.SetActive(false);
        yield return new WaitForSeconds(afterEffectDuration);
        Destroy(gameObject);
    }

}

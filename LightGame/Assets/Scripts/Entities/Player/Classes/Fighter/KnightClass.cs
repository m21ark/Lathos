using UnityEngine;
using System.Collections;


public class KnightClass : FighterClass
{
    public float A0StartOffset1 = 0.3f;
    public float A1StartOffset1 = 0.3f;
    public float A2StartOffset1 = 0.3f;
    public GameObject A2VFX;

    public override void Attack()
    {
        DelayAttackPhysical(A0Prefab, A0Damage, A0StartOffset1);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerKnightA0, transform.position, 0.15f);
    }

    public override void BaseAbility()
    {
        DelayAttackPhysical(A1Prefab, A1Damage, A1StartOffset1);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerKnightA1, transform.position, 0.2f);
    }

    public override void SpecialAbility()
    {   
        DelayAttackPhysical(A2Prefab, A2Damage, A2StartOffset1);
        StartCoroutine(GenerateVFXDimenstionalSlash());
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerKnightA2, transform.position, 0.3f);
    }

    private IEnumerator GenerateVFXDimenstionalSlash()
    {
        yield return new WaitForSeconds(A2StartOffset1);
        // Quaternion rotation = cameraPivot.transform.rotation;
        GameObject vfx = Instantiate(A2VFX, transform.position, Quaternion.identity);
    }
}

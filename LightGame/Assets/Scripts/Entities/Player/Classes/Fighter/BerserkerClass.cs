using UnityEngine;
using System.Collections;
using UnityEngine.VFX;

public class BerserkerClass : FighterClass
{
    private ProtoAttack attack;
    private Vector3 attackDirection;

    public float A1TimeDelta = 0.8f;
    public float A2TimeSpan = 3.0f;
    public float A2BuffMult = 1.5f;
    public float A2DebuffMult = 0.5f;

    public float A0StartOffset1 = 0.8f;
    public float A1StartOffset1 = 0.8f;

    public GameObject A2VFX;

    public override void Attack()
    {
        DelayAttackPhysical(A0Prefab, A0Damage, A0StartOffset1);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerBerserkerA0, transform.position, 0.15f);
    }

    public override void BaseAbility()
    {
        StartCoroutine(gameObject.GetComponent<PlayerController>().Dash());
        DelayAttackPhysical(A1Prefab, A1Damage, A1StartOffset1);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerBerserkerA1, transform.position, 0.3f);
    }

    public override void SpecialAbility()
    {
        GameObject vfx = Instantiate(A2VFX, transform.position, transform.rotation);
        BerserkLogic berserkLogic = vfx.GetComponent<BerserkLogic>();
        berserkLogic.berserkDuration = A2TimeSpan;
        // Get the Skinned Mesh Renderer
        SkinnedMeshRenderer skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        // Get VFX Component
        VisualEffect vfxComponent = vfx.GetComponent<VisualEffect>();
        // Set the Skinned Mesh Renderer to the VFX
        if (vfxComponent != null && skinnedMeshRenderer != null)
        {
            Debug.Log("Setting Skinned Mesh Renderer");
            vfxComponent.SetSkinnedMeshRenderer("SkinnedMeshRenderer", skinnedMeshRenderer);
        }
        StartCoroutine(ActivateSpecialAbility());
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerBerserkerA2, transform.position, 0.15f);
    }

    private IEnumerator ActivateSpecialAbility()
    {
        // Double armor and base damage
        float originalArmor = armor;
        int originalBaseDamage = A0Damage;

        armor *= A2DebuffMult;
        A0Damage = Mathf.RoundToInt(A0Damage * A2BuffMult);

        yield return new WaitForSeconds(A2TimeSpan);

        // Restore original armor and base damage
        armor = originalArmor;
        A0Damage = originalBaseDamage;
    }
}


using UnityEngine;
using System.Collections;

public class SharpshooterClass : RangerClass
{
    public GameObject VFXAbility;
    public GameObject VFXSpecialAbility;
    public override void Attack()
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttack(A0Prefab, out attack, out attackDirection);
        attack.Fire(A0Damage, attackDirection);
    }

    public override void BaseAbility()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerSharpshooterA1, transform.position);
        GenerateVFX(VFXAbility, 15);
    }

    public override void SpecialAbility()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerSharpshooterA2, transform.position);

        Vector3 offset = new Vector3(0, 0, 3);
        GenerateVFXOnPlayer(VFXSpecialAbility, transform, 15, offset);
        StartCoroutine(ActivateSpecialAbility());
    }

    private IEnumerator ActivateSpecialAbility()
    {
        GetComponent<PlayerController>().ForceCharacterFaceCrosshair();
        Vector3 fixedDirection = gameObject.transform.forward;

        yield return new WaitForSeconds(1.9f);
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttack(A2Prefab, out attack, out attackDirection);
        attack.Fire(A2Damage, attackDirection, ("attackDirection" , fixedDirection));
    }
}

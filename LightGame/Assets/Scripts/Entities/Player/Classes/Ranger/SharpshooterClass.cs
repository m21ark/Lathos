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

        Vector3 offset = GetComponent<PlayerController>().cameraPivot.forward * 3;

        Quaternion newRotation = GetComponent<PlayerController>().cameraPivot.rotation;

        GenerateVFX(VFXSpecialAbility, 15, offset, rotation: newRotation);
        StartCoroutine(ActivateSpecialAbility());
    }

    private IEnumerator ActivateSpecialAbility()
    {
        GetComponent<PlayerController>().ForceCharacterFaceCrosshair();
        Vector3 fixedDirection = GetComponent<PlayerController>().cameraPivot.forward;
        Quaternion cameraAngleRotation =  GetComponent<PlayerController>().cameraPivot.rotation;

        yield return new WaitForSeconds(1.9f);
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttack(A2Prefab, out attack, out attackDirection);

        attack.gameObject.transform.rotation = cameraAngleRotation;  

        attack.Fire(A2Damage, fixedDirection);
    }
}

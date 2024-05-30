using UnityEngine;

public class WizardClass : MageClass
{

    public GameObject A0_2Prefab;
    public GameObject A2_2Prefab;
    public GameObject A2_2VFX;

    public override void Attack() 
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttack(A0Prefab, out attack, out attackDirection);
        attack.Fire(A0Damage, attackDirection, ("prefab", A0_2Prefab));
    }

    public override void SpecialAbility()
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttack(A2Prefab, out attack, out attackDirection);
        attack.Fire(A2Damage, attackDirection, ("prefab", A2_2Prefab), ("vfx", A2_2VFX));
    }
}

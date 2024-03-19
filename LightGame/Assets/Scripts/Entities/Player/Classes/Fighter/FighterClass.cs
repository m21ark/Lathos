using UnityEngine;

public class FighterClass : BaseClass
{
    public override void BaseAbility()
    {
        GameObject attackEntity = Instantiate(classAttackPrefab, cameraPivot.position, Quaternion.identity);
        Vector3 attackDirection = cameraPivot.forward;
        ProtoAttack attack = attackEntity.GetComponent<ProtoAttack>();
        attack.Fire(30, attackDirection, 50, 0); // Hardcoded for now
    }
}

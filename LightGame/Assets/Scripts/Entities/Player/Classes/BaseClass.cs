using UnityEngine;

public class BaseClass : ProtoClass
{
    public override void Attack() 
    {
        GameObject attackEntity = Instantiate(simpleAttackPrefab, cameraPivot.position, Quaternion.identity);
        Vector3 attackDirection = cameraPivot.forward;
        ProtoAttack attack = attackEntity.GetComponent<ProtoAttack>();
        attack.Fire(10, attackDirection, 50, 0); // Hardcoded for now
     
    }
}

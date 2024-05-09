using UnityEngine;
using System.Collections;

public class SorcererClass : MageClass
{

    private ProtoAttack attack;
    private Vector3 attackDirection;
    public float A1TimeDelta = 0.8f;

    public override void Attack() 
    {
        ProtoAttack attack;
        Vector3 attackDirection;
        GenerateAttackAim(A0Prefab, out attack, out attackDirection);
        attack.Fire(A0Damage, attackDirection);
    }

    
    public override void BaseAbility()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Minion");

        // For each enemy, apply the damage based on the number of stacks and remove the tag
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<SorcererTag>())
            {
                ProtoMob mob = enemy.GetComponent<ProtoMob>();
                mob.TakeDamage(A1Damage * enemy.GetComponent<SorcererTag>().stackCounter);
                Destroy(enemy.GetComponent<SorcererTag>());
            }
        }
    }


    public override void SpecialAbility()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Minion");

        // For each enemy, add X stacks
        foreach (GameObject enemy in enemies)
        {
            if (!enemy.GetComponent<SorcererTag>())
                enemy.AddComponent<SorcererTag>();
            enemy.GetComponent<SorcererTag>().addStack(A2Damage);
        }

    }

}

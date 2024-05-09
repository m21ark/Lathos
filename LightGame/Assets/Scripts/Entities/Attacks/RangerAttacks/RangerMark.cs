using UnityEngine;

public class RangerMark : MonoBehaviour
{
    private int poisonTicksLeft = 3;
    private int damagePerTick = 10;
    private float tickTimer = 1f;
    
    void Update()
    {
        tickTimer -= Time.deltaTime;
        if (tickTimer <= 0f)
        {
            if (poisonTicksLeft > 0)
            {
                ProtoMob mob = this.GetComponent<ProtoMob>();
                mob.TakeDamage(damagePerTick);
                poisonTicksLeft--;
            }
            
            tickTimer = 1f;
        }
    }

    public void refreshTicks()
    {
        poisonTicksLeft = 3;
    }

    public void setValues(int ticks, int damage)
    {
        poisonTicksLeft = ticks;
        damagePerTick = damage;
    }
}
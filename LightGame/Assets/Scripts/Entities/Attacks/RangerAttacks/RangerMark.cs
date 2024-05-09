using UnityEngine;

public class RangerMark : MonoBehaviour
{
    public int poisonTicksLeft = 3;
    public int damagePerTick = 10;
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
            else Destroy(this.gameObject);
            
            tickTimer = 1f;
        }
    }

    public void refreshTicks()
    {
        poisonTicksLeft = 3;
    }
}
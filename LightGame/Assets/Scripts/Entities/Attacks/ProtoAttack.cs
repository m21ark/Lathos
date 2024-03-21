using UnityEngine;

public class ProtoAttack : MonoBehaviour
{
    
    public float gravityStrength = 1.0f;
    private float realGravity = 9.8f;
    protected int projDamage = 10;

    private Rigidbody attackRb;

    void Start(){}

    public virtual void Fire(int damage = 10, Vector3 direction = default(Vector3), float speed = 50.0f, float gravity = 0.0f, float despawnTime = 7.5f, GameObject prefab = null){

        attackRb = gameObject.GetComponent<Rigidbody>();

        if (attackRb != null)
            attackRb.velocity = direction * speed;

        gravityStrength = gravity;
        projDamage = damage;

        // Despawn attack after a while
        Destroy(gameObject.transform.parent.gameObject, despawnTime);

    }   


    void FixedUpdate()
    {
        attackRb = GetComponent<Rigidbody>();

        // Calculate the custom gravity vector
        Vector3 customGravity = -transform.up * gravityStrength * realGravity;

        // Apply the custom gravity force to the Rigidbody
        attackRb.AddForce(customGravity, ForceMode.Acceleration);
    }

    public virtual void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            return;
        }
        // Check if the attack is touching the ground
        if (collision.gameObject.CompareTag("Minion") || collision.gameObject.CompareTag("Boss"))
        {
           /// Get the Mob component
            ProtoMob mob = collision.gameObject.GetComponent<ProtoMob>();

            mob.TakeDamage(projDamage); 

        }

        // Destroy the attack
        Destroy(gameObject);

    }
}
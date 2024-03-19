using UnityEngine;

public class ProtoProjectile : MonoBehaviour
{
    
    private float gravityStrength = 1.0f;
    public float realGravity = 9.8f;
    private int projDamage = 10;

    private Rigidbody projectileRb;

    void Start(){}

    public void Fire(int damage = 10, Vector3 direction = default(Vector3), float speed = 1.0f, float gravity = 0.0f, GameObject projectile = null){
        
        if(projectile == null){
            Debug.Log("Tried to shot a null prefab");
            return;
        }

        projectileRb = projectile.GetComponent<Rigidbody>();

        if (projectileRb != null)
            projectileRb.velocity = direction * speed;

        gravityStrength = gravity;
        projDamage = damage;

        // Destroy the projectile after 5 seconds
        Destroy(projectile, 7.5f);
    }   

    public void FirePiu(GameObject projectilePrefab, Vector3 direction){
        Fire(10, direction, 80.0f, 0.0f, projectilePrefab);
    }

    void FixedUpdate()
    {
        projectileRb = GetComponent<Rigidbody>();

        // Calculate the custom gravity vector
        Vector3 customGravity = -transform.up * gravityStrength * realGravity;

        // Apply the custom gravity force to the Rigidbody
        projectileRb.AddForce(customGravity, ForceMode.Acceleration);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the projectile is touching the ground
        if (collision.gameObject.CompareTag("Minion"))
        {
           /// Get the Mob component
            ProtoMob mob = collision.gameObject.GetComponent<ProtoMob>();

            mob.TakeDamage(projDamage); 

            // Destroy the projectile
            Destroy(gameObject);
        }

        if(collision.gameObject.CompareTag("Boss")){
            // TODO: boss may have different behavior
        }
    }
}
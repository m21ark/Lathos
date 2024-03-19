using UnityEngine;

public class ProtoAttack : MonoBehaviour
{
    
    private float gravityStrength = 1.0f;
    public float realGravity = 9.8f;
    private int projDamage = 10;

    private Rigidbody attackRb;

    void Start(){}

    public void Fire(int damage = 10, Vector3 direction = default(Vector3), float speed = 1.0f, float gravity = 0.0f, GameObject attack = null){
        
        if(attack == null){
            Debug.Log("Tried to shot a null prefab");
            return;
        }

        attackRb = attack.GetComponent<Rigidbody>();

        if (attackRb != null)
            attackRb.velocity = direction * speed;

        gravityStrength = gravity;
        projDamage = damage;

        // Destroy the attack after 5 seconds
        Destroy(attack, 7.5f);
    }   

    public void FirePiu(GameObject attackPrefab, Vector3 direction){
        Fire(10, direction, 80.0f, 0.0f, attackPrefab);
    }

    void FixedUpdate()
    {
        attackRb = GetComponent<Rigidbody>();

        // Calculate the custom gravity vector
        Vector3 customGravity = -transform.up * gravityStrength * realGravity;

        // Apply the custom gravity force to the Rigidbody
        attackRb.AddForce(customGravity, ForceMode.Acceleration);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the attack is touching the ground
        if (collision.gameObject.CompareTag("Minion"))
        {
           /// Get the Mob component
            ProtoMob mob = collision.gameObject.GetComponent<ProtoMob>();

            mob.TakeDamage(projDamage); 

            // Destroy the attack
            Destroy(gameObject);
        }

        if(collision.gameObject.CompareTag("Boss")){
            // TODO: boss may have different behavior
        }
    }
}
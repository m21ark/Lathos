using UnityEngine;
using System.Linq;

public class ProtoAttack : MonoBehaviour
{
    
    // Attack attributes
    public float gravity = 1.0f;
    public float speed = 50.0f;
    public float despawnTime = 5.0f;
    
    // Attack collision behavior
    public bool ignorePlayerCol = true;
    public bool despawnOnCol = true;
    
    // Private values
    protected int projDamage = 10;
    private Rigidbody attackRb;
    private float realGravity = 9.8f;

    void Start(){}

    public virtual void Fire(int damage, Vector3 direction, params (string key, object value)[] kwargs){

        attackRb = gameObject.GetComponent<Rigidbody>();

        if (attackRb != null)
            attackRb.velocity = direction * speed;

        projDamage = damage;

        // Despawn attack when despawn time is reached
        Destroy(gameObject.transform.parent.gameObject, despawnTime);
    }   


    void FixedUpdate()
    {
        attackRb = GetComponent<Rigidbody>();

        // Calculate the custom gravity vector
        Vector3 customGravity = -transform.up * gravity * realGravity;

        // Apply the custom gravity force to the Rigidbody
        attackRb.AddForce(customGravity, ForceMode.Acceleration);
    }

    public virtual void OnTriggerEnter(Collider collision)
    {

        if(ignorePlayerCol && collision.gameObject.CompareTag("Player"))
            return;
        
        // Check if the attack hit enemy and damage it
        if (collision.gameObject.CompareTag("Minion") || collision.gameObject.CompareTag("Boss"))
        {
           /// Get the Mob component
            ProtoMob mob = collision.gameObject.GetComponent<ProtoMob>();

            mob.TakeDamage(projDamage); 

        }

        // Destroy the attack
        if(despawnOnCol)
            Destroy(gameObject);
    }

    public object GetKwarg(string keyName, params (string key, object value)[] kwargs){
        return kwargs.FirstOrDefault(pair => pair.key == keyName).value;
    }
}
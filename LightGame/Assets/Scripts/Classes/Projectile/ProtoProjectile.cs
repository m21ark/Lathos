using UnityEngine;

public class ProtoProjectile : MonoBehaviour
{
    

    public GameObject projectilePrefab;
    public void Fire(int damage, Vector3 direction, float speed, float gravity, Vector3 cameraForward){
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        if (projectileRb != null){
            projectileRb.velocity = cameraForward * speed;
            projectileRb.useGravity = true;
        }
    }   

    public void Despawn(){
        Destroy(gameObject);
    }
}
70 25 15 30 1

   void ShootProjectile()
    {
        float projectileSpeed = 50f;

        // Calculate the direction of the camera
        Vector3 cameraDirection = cameraPivot.forward;

        // Instantiate the projectile slightly in front of the player
        GameObject projectile = Instantiate(projectilePrefab, transform.position + cameraDirection, Quaternion.identity);

        // Get the rigidbody of the projectile
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // If the projectile has a rigidbody component, apply velocity in the camera direction
        if (projectileRb != null)
            projectileRb.velocity = cameraDirection * projectileSpeed;
    }
using UnityEngine;

public class BaseClass : ProtoClass
{
    public override void Attack()
    {
        // Here you can decide which attack type to use
        // Then load the appropriate prefab by name
        string prefabName = GetProjectilePrefabNameForCurrentAttackType();

        if (!string.IsNullOrEmpty(prefabName))
        {
            GameObject projectilePrefab = Resources.Load<GameObject>(prefabName);

            if (projectilePrefab != null)
            {
                GameObject projectileObject = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                ProtoProjectile instantiatedProjectile = projectileObject.GetComponent<ProtoProjectile>();

                if (instantiatedProjectile != null)
                {
                    instantiatedProjectile.Fire();
                }
                else
                {
                    Debug.LogError("Projectile prefab does not contain ProtoProjectile component.");
                }
            }
            else
            {
                Debug.LogError("Projectile prefab with name '" + prefabName + "' not found.");
            }
        }
        else
        {
            Debug.LogError("No projectile prefab name assigned for the current attack type.");
        }
    }

    private string GetProjectilePrefabNameForCurrentAttackType()
    {
        // Here you would implement logic to determine which attack type to use,
        // and return the corresponding prefab name
        // For example, you might have a property or method to get the current attack type
        // and use that to decide which prefab name to return
        // return baseProjectilePrefabName; // Change this to return the appropriate prefab name

        return "BaseProjectile";
    }
}

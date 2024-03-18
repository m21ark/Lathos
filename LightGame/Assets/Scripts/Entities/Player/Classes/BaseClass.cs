using UnityEngine;

public class BaseClass : ProtoClass
{
    public void Attack(Vector3 direction, Vector3 position)
    {
        // Here you can decide which attack type to use
        // Then load the appropriate prefab by name
        string prefabName = GetProjectilePrefabNameForCurrentAttackType();

        if (!string.IsNullOrEmpty(prefabName))
        {
            GameObject projectilePrefab = Resources.Load<GameObject>(prefabName);

            if (projectilePrefab != null)
            {
                GameObject projectileObject = Instantiate(projectilePrefab, position + direction, Quaternion.identity);
                ProtoProjectile instantiatedProjectile = projectileObject.GetComponent<ProtoProjectile>();

                if (instantiatedProjectile != null)  instantiatedProjectile.FirePiu(projectileObject, direction);
                else Debug.LogError("Projectile prefab does not contain ProtoProjectile component.");   
            }
            else Debug.LogError("Projectile prefab with name '" + prefabName + "' not found.");
            
        }
        else Debug.LogError("No projectile prefab name assigned for the current attack type.");
        
    }

    private string GetProjectilePrefabNameForCurrentAttackType()
    {
        return "BaseProjectile"; // Name of Prefab on Resources folder
    }
}

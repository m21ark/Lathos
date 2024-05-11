using UnityEngine;

public class DieOnCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            ProtoClass player = collision.gameObject.GetComponent<ProtoClass>();
            if (player != null)
            {
                player.TakeDamage(100);
            }
        }
    }
}

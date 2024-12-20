using UnityEngine;

public class PlayerHitHandler : MonoBehaviour
{
    [SerializeField]
    private float UpForce = 1f; // Y Axis force on player
    [SerializeField]
    private float BackForce = 10f; // X Axis force on player
    [SerializeField]
    private float damageHit = 1f; //Damage made to player

    private PlayerHealth playerHealth;

    private void OnTriggerEnter(Collider other)
    {
        // If The Weapon hit the Player then Player takes damage
        if (other.CompareTag("Player"))
        {
            // Player takes damage
            playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damageHit, UpForce, BackForce);
        }
    }

    
}

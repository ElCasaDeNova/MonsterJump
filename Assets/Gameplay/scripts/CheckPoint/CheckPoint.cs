using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is the player
        if (other.CompareTag("Player"))
        {
            // Find all objects with the RespawnZone script
            RespawnHandler[] respawnZones = FindObjectsOfType<RespawnHandler>();

            // Call UpdateCheckpoint on each one
            foreach (RespawnHandler respawnZone in respawnZones)
            {
                respawnZone.UpdateCheckpoint(this.gameObject);
            }
        }
    }

}

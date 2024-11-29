using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is the player
        if (other.CompareTag("Player"))
        {
            // Find all objects with the RespawnZone script
            RespawnZone[] respawnZones = FindObjectsOfType<RespawnZone>();

            // Call UpdateCheckpoint on each one
            foreach (RespawnZone respawnZone in respawnZones)
            {
                respawnZone.UpdateCheckpoint(this.gameObject);
            }
        }
    }

}

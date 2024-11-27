using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is the player
        if (other.CompareTag("Player"))
        {
            // Call the UpdateCheckpoint method on the RespawnZone
            RespawnZone respawnZone = FindObjectOfType<RespawnZone>();
            if (respawnZone != null)
            {
                respawnZone.UpdateCheckpoint(this.gameObject);
                Debug.Log("Player reached checkpoint: " + this.name);
            }
        }
    }
}

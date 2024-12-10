using UnityEngine;

public class RespawnZone : MonoBehaviour
{
    // Variable to store the last checkpoint
    [SerializeField]
    private GameObject lastCheckpoint;

    // Called when another Collider enters the respawn zone
    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is the player
        if (other.CompareTag("Player"))
        {
            RespawnPlayer(other.gameObject);
        }
    }

    // Function to teleport the player to the last checkpoint
    private void RespawnPlayer(GameObject player)
    {
        if (lastCheckpoint != null)
        {
            // Teleport the player to the last checkpoint's position
            player.transform.position = lastCheckpoint.transform.position;
            player.transform.rotation = lastCheckpoint.transform.rotation;

            // Reset the player's velocity if they have a Rigidbody
            Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                playerRigidbody.velocity = Vector3.zero;
            }

            // Get Camera Handler Script
            MovementControls movementControls = player.GetComponent<MovementControls>();
            if (movementControls != null)
            {
                Debug.Log(player.transform.rotation.eulerAngles.y);
                movementControls.ResetCameraPosition(player.transform.rotation.eulerAngles.y); // Update Camera Rotation
            }

            Debug.Log("Player respawned at checkpoint: " + lastCheckpoint.name);
        }
        else
        {
            Debug.LogWarning("No checkpoint set. Respawn failed!");
        }
    }

    // Updates the last checkpoint (called by the checkpoint)
    public void UpdateCheckpoint(GameObject newCheckpoint)
    {
        lastCheckpoint = newCheckpoint;
        Debug.Log("Checkpoint updated to: " + lastCheckpoint.name);
    }
}

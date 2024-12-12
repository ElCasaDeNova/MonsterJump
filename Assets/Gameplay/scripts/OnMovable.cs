using UnityEngine;

public class OnMovable : MonoBehaviour
{
    private Transform platform; // Reference to the moving platform
    private Vector3 lastPlatformPosition; // Previous position of the platform
    private Rigidbody playerRb; // Rigidbody of the player
    private bool isOnPlatform; // Indicates if the player is on a moving platform

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if we collide with a moving platform
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            platform = collision.transform;
            lastPlatformPosition = platform.position;
            isOnPlatform = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (platform != null && isOnPlatform)
        {
            // Calculate the movement of the platform
            Vector3 platformMovement = platform.position - lastPlatformPosition;

            // Move the player along with the platform
            playerRb.MovePosition(playerRb.position + platformMovement);

            // Update the previous position of the platform
            lastPlatformPosition = platform.position;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Exit the moving platform
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            platform = null;
            isOnPlatform = false;
        }
    }
}

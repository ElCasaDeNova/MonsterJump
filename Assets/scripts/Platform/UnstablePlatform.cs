using UnityEngine;

public class UnstablePlatform : MonoBehaviour
{
    private bool isPlayerOnPlatform = false; // To track if the player is on the platform
    private float timeOnPlatform = 0f; // Timer to track how long the player has been off the platform

    [SerializeField]
    private float returnDelay = 1f; // Time before platform starts to rotate back to 0
    [SerializeField]
    private float rotationSpeed = 1f; // Speed at which the platform will rotate back to 0

    private float initialZRotation; // Store the initial Z rotation of the platform
    private float targetZRotation = 0f; // Target Z rotation (0 degrees)
    private bool isReturning = false; // Flag to check if the platform should start returning

    private void Start()
    {
        // Store the initial Z rotation of the platform
        initialZRotation = transform.rotation.eulerAngles.z;
    }

    private void Update()
    {
        // If the player is no longer on the platform, we start the timer
        if (!isPlayerOnPlatform)
        {
            timeOnPlatform += Time.deltaTime;

            // If the player has been off the platform for the specified delay, start rotation towards 0
            if (timeOnPlatform >= returnDelay && !isReturning)
            {
                isReturning = true;
            }
        }

        // If the platform should return to its original Z rotation
        if (isReturning)
        {
            // Smoothly rotate the platform to the target Z rotation (0 degrees)
            float currentZRotation = transform.rotation.eulerAngles.z;

            // Determine the direction to rotate
            float step = rotationSpeed * Time.deltaTime;

            // Calculate the new Z rotation and apply it smoothly
            float newZRotation = Mathf.MoveTowardsAngle(currentZRotation, targetZRotation, step);

            // Update the platform's rotation
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, newZRotation);

            // If the Z rotation has reached 0, stop the movement
            if (Mathf.Approximately(newZRotation, targetZRotation))
            {
                isReturning = false;
            }
        }
    }

    // Called when the player enters the platform's trigger area
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player is on the platform
            isPlayerOnPlatform = true;
            timeOnPlatform = 0f; // Reset the timer
            isReturning = false; // Stop returning rotation if the player is back on the platform
        }
    }

    // Called when the player leaves the platform's trigger area
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player is off the platform
            isPlayerOnPlatform = false;
        }
    }
}

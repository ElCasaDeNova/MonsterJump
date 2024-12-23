using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [SerializeField]
    private Transform pointA;
    [SerializeField]
    private Transform pointB;
    [SerializeField]
    private float speed = 2f; // Movement speed
    [SerializeField]
    private float smoothTime = 0.3f; // Smooth time for the movement
    [SerializeField]
    private float stopDistance = 0.05f; // Distance at which the platform is considered to have arrived

    private Transform target; // Destination point
    private Vector3 velocity = Vector3.zero; // Velocity for SmoothDamp

    void Start()
    {
        // Initialize the target to the first point
        target = pointB;
    }

    void Update()
    {
        // Get the transform of the platform
        Transform platform = transform.GetChild(0); // First child

        // Move the platform with smooth movement
        platform.position = Vector3.SmoothDamp(platform.position, target.position, ref velocity, smoothTime, speed);

        // Check if the platform has reached its target with more precise stopping distance
        if (Vector3.Distance(platform.position, target.position) < stopDistance)
        {
            // Switch the target
            target = (target == pointA) ? pointB : pointA;
        }
    }
}

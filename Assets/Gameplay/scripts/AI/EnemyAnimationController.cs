using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationController : MonoBehaviour
{
    public Animator animator; // Reference to the Animator component
    public float inputSmoothSpeed = 5f; // Speed of smoothing for animation transitions
    public float inputDeadZone = 0.1f; // Threshold to consider an input as zero

    private NavMeshAgent agent; // Reference to the NavMeshAgent
    private Vector3 previousPosition; // To calculate movement direction
    private float smoothedInputX; // Smoothed horizontal input
    private float smoothedInputY; // Smoothed vertical input

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (animator == null)
        {
            Debug.LogError("Animator is not assigned!");
        }
        previousPosition = transform.position;
    }

    void Update()
    {
        // Only update animation inputs if the game is not paused
        if (Time.timeScale > 0f)
        {
            UpdateAnimationInputs();
        }
    }

    private void UpdateAnimationInputs()
    {
        // Calculate the agent's movement direction based on its velocity
        Vector3 velocity = (transform.position - previousPosition) / Time.deltaTime; // Approximation of velocity
        previousPosition = transform.position;

        // Normalize velocity to get directional input
        Vector3 localVelocity = transform.InverseTransformDirection(velocity); // Transform to local space
        float inputX = localVelocity.x;
        float inputY = localVelocity.z;

        // Smooth inputs for better animation transitions
        smoothedInputX = Mathf.Lerp(smoothedInputX, inputX, Time.deltaTime * inputSmoothSpeed);
        smoothedInputY = Mathf.Lerp(smoothedInputY, inputY, Time.deltaTime * inputSmoothSpeed);

        // Apply a dead zone to avoid jittering at low speeds
        if (Mathf.Abs(smoothedInputX) < inputDeadZone) smoothedInputX = 0f;
        if (Mathf.Abs(smoothedInputY) < inputDeadZone) smoothedInputY = 0f;

        // Pass the smoothed inputs to the animator
        animator.SetFloat("InputX", smoothedInputX);
        animator.SetFloat("InputY", smoothedInputY);
    }
}

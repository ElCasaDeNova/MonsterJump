using UnityEngine;

// Enum to define the different states of the enemy
public enum EnemyState { Patrol, Chase }

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    private EnemyState currentState; // Current state of the enemy

    [Header("Vision Settings")]
    public float viewDistance = 10f; // Maximum distance the enemy can see
    public float viewAngle = 45f; // Angle of the enemy's field of view
    public LayerMask obstacleMask; // Layers considered as obstacles for vision

    private EnemyPatrol patrolScript; // Reference to the EnemyPatrol script
    private EnemyChase chaseScript; // Reference to the EnemyChase script

    void Start()
    {
        // Get references to the patrol and chase scripts
        patrolScript = GetComponent<EnemyPatrol>();
        chaseScript = GetComponent<EnemyChase>();

        // Start in patrol mode
        currentState = EnemyState.Patrol;
        EnablePatrol();
    }

    void Update()
    {
        // If the player is visible, switch to chase mode
        if (CanSeePlayer())
        {
            if (currentState != EnemyState.Chase)
            {
                currentState = EnemyState.Chase;
                EnableChase();
            }
        }
    }

    void EnablePatrol()
    {
        patrolScript.enabled = true;  // Activate patrol script
        chaseScript.enabled = false;  // Deactivate chase script
    }

    void EnableChase()
    {
        patrolScript.enabled = false; // Deactivate patrol script
        chaseScript.enabled = true;   // Activate chase script
    }

    bool CanSeePlayer()
    {
        // Calculate direction to the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Check if the player is within the field of view
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer < viewAngle / 2f)
        {
            // Check if the player is within viewing distance
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= viewDistance)
            {
                // Check for obstacles between the enemy and the player
                if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleMask))
                {
                    return true; // Player is visible
                }
            }
        }

        return false; // Player is not visible
    }
}

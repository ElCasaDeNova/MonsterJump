using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints; // Array of points the enemy will patrol between
    private int currentPatrolIndex; // Index of the current patrol point
    private NavMeshAgent agent; // Reference to the NavMeshAgent component

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // Set the first patrol destination
        if (patrolPoints.Length > 0)
        {
            agent.destination = patrolPoints[0].position;
        }
    }

    void Update()
    {
        // Check if the enemy has reached the current destination
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            // Move to the next patrol point (loop back to the first if necessary)
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.destination = patrolPoints[currentPatrolIndex].position;
        }
    }
}

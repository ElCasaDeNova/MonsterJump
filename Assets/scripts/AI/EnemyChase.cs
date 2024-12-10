using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    private NavMeshAgent agent; // Reference to the NavMeshAgent component

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Toujours suivre le joueur en mode poursuite
        if (agent.isActiveAndEnabled)
        {
            agent.destination = player.position;
        }
    }
}

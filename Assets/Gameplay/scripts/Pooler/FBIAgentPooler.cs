using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBIAgentPooler : MonoBehaviour
{
    public GameObject fbiAgentPrefab; 
    public int poolSize = 50;

    private Queue<GameObject> pool;

    void Awake()
    {
        // Initialize pool
        pool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject agent = Instantiate(fbiAgentPrefab);
            agent.SetActive(false);
            pool.Enqueue(agent);
        }
    }

    public GameObject GetAgent(Vector3 position, Quaternion rotation)
    {
        if (pool.Count == 0)
        {
            // Extend pool if necessary
            GameObject extraAgent = Instantiate(fbiAgentPrefab);
            pool.Enqueue(extraAgent);
        }

        GameObject agentToSpawn = pool.Dequeue();
        agentToSpawn.SetActive(true);
        agentToSpawn.transform.position = position;
        agentToSpawn.transform.rotation = rotation;

        return agentToSpawn;
    }

    public void ReturnAgent(GameObject agent)
    {
        agent.SetActive(false);
        pool.Enqueue(agent);
    }
}

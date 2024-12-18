using System.Collections.Generic;
using UnityEngine;

public class FBIRobotPooler : MonoBehaviour
{
    public GameObject fbiRobotPrefab;
    public int poolSize = 50;

    private Queue<GameObject> pool;

    void Awake()
    {
        // Initialize pool
        pool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject robot = Instantiate(fbiRobotPrefab);
            robot.SetActive(false);
            pool.Enqueue(robot);
        }
    }

    public GameObject GetObject(Vector3 position, Quaternion rotation)
    {
        if (pool.Count == 0)
        {
            // Extend pool if necessary
            GameObject extraRobot = Instantiate(fbiRobotPrefab);
            pool.Enqueue(extraRobot);
        }

        GameObject robotToSpawn = pool.Dequeue();
        robotToSpawn.SetActive(true);
        robotToSpawn.transform.position = position;
        robotToSpawn.transform.rotation = rotation;

        return robotToSpawn;
    }

    public void ReturnObject(GameObject robot)
    {
        robot.SetActive(false);
        pool.Enqueue(robot);
    }
}

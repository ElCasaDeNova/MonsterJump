using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public GameObject prefab; // Assignable prefab
    public int poolSize = 50;

    private Queue<GameObject> pool;

    void Awake()
    {
        // Initialize pool
        pool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetObject(Vector3 position, Quaternion rotation)
    {
        if (pool.Count == 0)
        {
            // Extend pool if necessary
            GameObject extraObj = Instantiate(prefab);
            pool.Enqueue(extraObj);
        }

        GameObject objToSpawn = pool.Dequeue();
        objToSpawn.SetActive(true);
        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;

        return objToSpawn;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}

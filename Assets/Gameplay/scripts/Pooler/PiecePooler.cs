using System.Collections.Generic;
using UnityEngine;

public class PiecePooler : MonoBehaviour
{
    [Header("Coin Parameters")]
    [SerializeField]
    private GameObject bluePiecePrefab; // Prefab for the blue coin
    [SerializeField]
    private GameObject redPiecePrefab;  // Prefab for the red coin
    [SerializeField]
    private int numberOfBluePieces = 4; // Total number of blue coins
    [SerializeField]
    private int numberOfRedPieces = 2;  // Total number of red coins

    [SerializeField]
    private GameObject Spawners;

    private List<Transform> spawnPoints = new List<Transform>(); // List of spawn point positions
    private List<GameObject> coinPool = new List<GameObject>(); // Pool containing the coins to instantiate


    private void Start()
    {
        if (Spawners != null)
        {
            // Get All Spawners positions
            GetAllChildren(Spawners);
        }
        else
        {
            Debug.LogError("Spawners is not assigned!");
        }

        // Checkif the coins match the spawners
        if (numberOfBluePieces + numberOfRedPieces != spawnPoints.Count)
        {
            Debug.LogError("Error: The total number of coins must equal the number of spawners!");
            return;
        }

        // Step 1: Create the coin pool
        CreateCoinPool();

        // Step 2: Shuffle the spawners
        ShuffleSpawnPoints();

        // Step 3: Distribute the coins on the spawners
        SpawnCoins();
    }

    private void GetAllChildren(GameObject parent)
    {
        // For Each Child
        foreach (Transform child in parent.transform)
        {
            // Add child in the list
            spawnPoints.Add(child.gameObject.transform);
        }
    }

    // Step 1: Create a pool containing BluePieces and RedPieces
    private void CreateCoinPool()
    {
        coinPool.Clear();

        // Add blue coins to the pool
        for (int i = 0; i < numberOfBluePieces; i++)
        {
            coinPool.Add(bluePiecePrefab);
        }

        // Add red coins to the pool
        for (int i = 0; i < numberOfRedPieces; i++)
        {
            coinPool.Add(redPiecePrefab);
        }

        // Shuffle the pool to randomize the types of coins
        ShuffleList(coinPool);
    }

    // Step 2: Shuffle the spawners to randomize their order
    private void ShuffleSpawnPoints()
    {
        ShuffleList(spawnPoints);
    }

    // Step 3: Instantiate the coins at the spawner positions
    private void SpawnCoins()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            // Instantiate each coin at the spawner's position
            Instantiate(coinPool[i], spawnPoints[i].position, Quaternion.identity);
        }
    }

    // Function to shuffle a list using the Fisher-Yates Shuffle
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void ReturnCoinToPool(GameObject coin)
    {
        // Deactivate the coin to return it to the pool
        coin.SetActive(false);
    }
}

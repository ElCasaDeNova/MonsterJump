using System.Collections.Generic;
using UnityEngine;

public class PiecePooler : MonoBehaviour
{
    [Header("Piece Parameters")]
    [SerializeField]
    private GameObject bluePiecePrefab; // Prefab for the blue Piece
    [SerializeField]
    private GameObject redPiecePrefab;  // Prefab for the red Piece
    [SerializeField]
    private int numberOfBluePieces = 4; // Total number of blue Pieces
    [SerializeField]
    private int numberOfRedPieces = 2;  // Total number of red Pieces

    [SerializeField]
    private GameObject Spawners;

    private List<Transform> spawnPoints = new List<Transform>(); // List of spawn point positions
    private List<GameObject> piecePool = new List<GameObject>(); // Pool containing the Pieces to instantiate


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

        // Checkif the Pieces match the spawners
        if (numberOfBluePieces + numberOfRedPieces != spawnPoints.Count)
        {
            Debug.LogError("Error: The total number of Pieces must equal the number of spawners!");
            return;
        }

        // Step 1: Create the Piece pool
        CreatePiecePool();

        // Step 2: Shuffle the spawners
        ShuffleSpawnPoints();

        // Step 3: Distribute the Pieces on the spawners
        SpawnPieces();
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
    private void CreatePiecePool()
    {
        piecePool.Clear();

        // Add blue Pieces to the pool
        for (int i = 0; i < numberOfBluePieces; i++)
        {
            piecePool.Add(bluePiecePrefab);
        }

        // Add red Pieces to the pool
        for (int i = 0; i < numberOfRedPieces; i++)
        {
            piecePool.Add(redPiecePrefab);
        }

        // Shuffle the pool to randomize the types of Pieces
        ShuffleList(piecePool);
    }

    // Step 2: Shuffle the spawners to randomize their order
    private void ShuffleSpawnPoints()
    {
        ShuffleList(spawnPoints);
    }

    // Step 3: Instantiate the Pieces at the spawner positions
    private void SpawnPieces()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            // Instantiate each Piece at the spawner's position and set the spawner as the parent
            GameObject piece = Instantiate(piecePool[i], spawnPoints[i].position, Quaternion.identity, spawnPoints[i]);

            // Get the PieceManager component of the instantiated piece and set the PiecePooler
            PieceManager pieceManager = piece.GetComponent<PieceManager>();
            if (pieceManager != null)
            {
                pieceManager.SetPiecePooler(this);  // Assign the PiecePooler to the piece
            }
            else
            {
                Debug.LogError("PieceManager component missing on " + piece.name);
            }
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

    public void ReturnPieceToPool(GameObject piece)
    {
        // Deactivate the Piece to return it to the pool
        piece.SetActive(false);
    }
}

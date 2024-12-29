using UnityEngine;
using UnityEngine.AI;

public class OpenDoor : MonoBehaviour
{
    [SerializeField]
    private GameObject enemies;
    [SerializeField]
    private GameObject door;    // Door to Open
    [SerializeField]
    private float moveDistance = 5f;     // Distance to move
    [SerializeField]
    private float moveSpeed = 2f;       // Speed of movement
    private GameObject rightDoor;
    private GameObject leftDoor;

    private void Start()
    {
        rightDoor = door.transform.Find("DoorRight")?.gameObject;
        leftDoor = door.transform.Find("DoorLeft")?.gameObject;
    }

    void Update()
    {
        if (enemies != null)
        {
            // Get all child transforms of the "Enemies" GameObject
            Transform[] children = enemies.GetComponentsInChildren<Transform>(true);

            // Check if all enemies are disabled
            bool allEnemiesDisabled = true; // Assume all are disabled initially
            foreach (Transform child in children)
            {
                if (child != enemies.transform) // Skip the parent itself
                {
                    // Check if the child has a component and is enabled
                    NavMeshAgent enemy = child.GetComponent<NavMeshAgent>(); // Replace "Enemy" with the script/component you are using
                    if (enemy != null && enemy.enabled) // If the enemy component exists and is enabled
                    {
                        allEnemiesDisabled = false; // Found an active enemy
                        break;
                    }
                }
            }

            // If all enemies are disabled, print the message and call the method
            if (allEnemiesDisabled)
            {
                //Debug.Log("Door is Opening");
                OpenNextDoor();
            }
        }
        else
        {
            Debug.LogError("GameObject 'Enemies' is not assigned or not found in the scene!");
        }
    }

    private void OpenNextDoor()
    {
        if (rightDoor != null && leftDoor != null)
        {
            StartCoroutine(MoveDoor(leftDoor, Vector3.right * moveDistance));
            StartCoroutine(MoveDoor(rightDoor, Vector3.left * moveDistance));
        }
    }

    private System.Collections.IEnumerator MoveDoor(GameObject door, Vector3 direction)
    {
        Vector3 startPosition = door.transform.position;
        Vector3 targetPosition = startPosition + direction;

        float elapsedTime = 0f;
        while (elapsedTime < moveDistance / moveSpeed)
        {
            door.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / (moveDistance / moveSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure door ends at the exact target position
        door.transform.position = targetPosition;
    }
}

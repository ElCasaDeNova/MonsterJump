using UnityEngine;

public class MemoryHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject lastMap;
    [SerializeField]
    private GameObject nextMap;

    [SerializeField]
    private GameObject nextRightDoor;
    [SerializeField]
    private GameObject nextLeftDoor;

    [SerializeField]
    private GameObject lastRightDoor;
    [SerializeField]
    private GameObject lastLeftDoor;

    [SerializeField]
    private float moveDistance = 5f;     // Distance to move
    [SerializeField]
    private float moveSpeed = 2f;       // Speed of movement

    private void OnTriggerEnter(Collider other)
    {
        // If The Weapon hit the Player then Player takes damage
        if (other.CompareTag("Player"))
        {
            // Generate Next Map
            if (nextMap != null)
            {
                EnableMap();
            }

            OpenNextDoor();

            CloseLastDoor();

            BoxCollider boxCollider = GetComponent<BoxCollider>();
            if (boxCollider != null)
            {
                boxCollider.enabled = false; // // DeActivate BoxCollider
            }
        }
    }

    private void OpenNextDoor()
    {
        if (nextRightDoor != null && nextLeftDoor != null)
        {
            StartCoroutine(MoveDoor(nextRightDoor, Vector3.left * moveDistance));
            StartCoroutine(MoveDoor(nextLeftDoor, Vector3.left * moveDistance));
        }
    }

    private void CloseLastDoor()
    {
        if (lastRightDoor != null && lastLeftDoor != null)
        {
            StartCoroutine(MoveDoor(lastLeftDoor, Vector3.left * moveDistance));
            StartCoroutine(MoveDoor(lastRightDoor, Vector3.right * moveDistance));
        }
    }

    private void DisableMap() {
        lastMap.SetActive(false);
    }

    private void EnableMap()
    {
        nextMap.SetActive(true);
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


        // Save some Memory by deleting last map
        if (lastMap != null)
        {
            DisableMap();
        }
    }
}

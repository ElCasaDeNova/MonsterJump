using UnityEngine;

public class HandTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           Debug.Log("Monster is touched");
        }
    }
}

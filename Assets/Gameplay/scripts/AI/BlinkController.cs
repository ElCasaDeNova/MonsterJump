using System.Collections.Generic;
using UnityEngine;

public class BlinkController : MonoBehaviour
{
    [SerializeField]
    private float blinkDuration = 2f; // time of blink
    [SerializeField]
    private float blinkCount = 3f;// number of blink
    [SerializeField]
    private float delayTime = 5f; // time before blink

    [SerializeField]
    private ObjectPooler pooler;

    private List<GameObject> children;

    private void Start()
    {
        // List to store child gameobjects
        children = new List<GameObject>();

        // Iterate over all children of the GameObject
        foreach (Transform child in transform)
        {
            // Add the child GameObject to the list
            children.Add(child.gameObject);
        }
    }

    public void Blink()
    {
        float blinkInterval = blinkDuration / blinkCount;
        Debug.Log("blinkInterval is " + blinkInterval);

        // Wait for delayTime before starting the blinking
        InvokeRepeating("Blinking", delayTime, blinkInterval);

        Invoke("StopBlinking", delayTime + blinkDuration);

    }

    public void Blinking()
    {
        foreach (GameObject child in children)
        {
            child.gameObject.SetActive(!child.gameObject.activeInHierarchy);
        }

        Debug.Log("Blinking!");
    }

    private void StopBlinking()
    {
        // Stop the repeating Blink method after the total blink duration
        CancelInvoke("Blinking");

        // Return the agent to the pool
        Debug.Log("waiting for " + blinkDuration + " seconds");
        pooler.ReturnObject(this.gameObject);
    }
}

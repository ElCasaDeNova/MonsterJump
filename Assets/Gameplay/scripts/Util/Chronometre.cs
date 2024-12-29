using UnityEngine;

public class Chronometer : MonoBehaviour
{
    public float elapsedTime = 0f; // Time elapsed in seconds
    private bool isChronometerActive = false;

    void Update()
    {
        if (isChronometerActive)
        {
            elapsedTime += Time.deltaTime; // Increment elapsed time  
            //Debug.Log(elapsedTime);
        }
    }

    // Method to start the chronometer
    public void StartChronometer()
    {
        isChronometerActive = true;
    }

    // Method to stop the chronometer
    public void StopChronometer()
    {
        isChronometerActive = false;
    }

    // Method to reset the chronometer
    public void ResetChronometer()
    {
        isChronometerActive = false;
        elapsedTime = 0f;
    }
}

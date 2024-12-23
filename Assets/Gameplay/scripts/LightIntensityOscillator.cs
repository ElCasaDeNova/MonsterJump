using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightIntensityOscillator : MonoBehaviour
{
    [Header("Oscillation Settings")]
    [SerializeField]
    private float minIntensity = 0.5f; // Minimum intensity
    [SerializeField]
    private float maxIntensity = 2.0f; // Maximum intensity
    [SerializeField]
    private float frequency = 1.0f;    // Frequency in oscillations per second

    private Light pointLight;           // Light component
    private float timeElapsed;

    void Start()
    {
        // Get the light component attached to this GameObject
        pointLight = GetComponent<Light>();

        // Check if the light type is Point
        if (pointLight.type != LightType.Point)
        {
            Debug.LogError("This script must be attached to a Point Light.");
        }
    }

    void Update()
    {
        if (pointLight.type == LightType.Point)
        {
            // Increment elapsed time
            timeElapsed += Time.deltaTime;

            // Calculate a sinusoidal oscillation
            float oscillation = Mathf.Sin(timeElapsed * frequency * 2 * Mathf.PI) * 0.5f + 0.5f;

            // Map the oscillation to the min/max intensity range
            pointLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, oscillation);
        }
    }
}

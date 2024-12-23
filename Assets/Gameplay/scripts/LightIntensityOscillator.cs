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

    private Light areaLight;
    private float timeElapsed;

    void Start()
    {
        // Get the light component attached to this GameObject
        areaLight = GetComponent<Light>();

        // Check if the light type is Area
        if (areaLight.type != LightType.Area)
        {
            Debug.LogError("This script must be attached to an Area Light.");
        }
    }

    void Update()
    {
        if (areaLight.type == LightType.Area)
        {
            // Increment elapsed time
            timeElapsed += Time.deltaTime;

            // Calculate a sinusoidal oscillation
            float oscillation = Mathf.Sin(timeElapsed * frequency * 2 * Mathf.PI) * 0.5f + 0.5f;

            // Map the oscillation to the min/max intensity range
            areaLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, oscillation);
        }
    }
}

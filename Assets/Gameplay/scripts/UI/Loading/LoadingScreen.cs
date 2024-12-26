using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField]
    private Slider loadingSlider;
    [SerializeField]
    private LoadingBarController loadingBarController;

    [SerializeField]
    private AnimationCurve loadingCurve;

    private void Start()
    {
        StartCoroutine(LoadGameAsync(SceneLoader.nextScene));  // Start loading the scene asynchronously
    }

    private IEnumerator LoadGameAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);  // Load the scene asynchronously
        operation.allowSceneActivation = false;  // Prevent scene from being activated immediately

        float elapsedTime = 0f;  // Time elapsed during the loading process
        float smoothProgress = 0f;  // The smooth progress of the loading bar

        while (!operation.isDone)
        {
            // Normalize the progress between 0 and 1 (up to 90%)
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            // Use the loading curve to simulate the loading speed
            float adjustedProgress = ApplyCustomLoadingCurve(progress);

            // Smooth the progress to fill the loading bar over time
            elapsedTime += Time.deltaTime;
            smoothProgress = Mathf.Lerp(smoothProgress, adjustedProgress, 0.1f);  // Lerp for smooth progress

            // Gradually increase the slider value
            loadingSlider.value = smoothProgress;
            loadingBarController.UpdateLoadingBar(smoothProgress);  // Update the text with the smooth progress

            // When the scene reaches 0.9, allow scene activation and show 100% on the bar after a slight delay
            if (operation.progress >= 0.9f)
            {
                loadingBarController.UpdateLoadingBar(1f);  // Set the loading bar to 100%
                loadingSlider.value = 1f;  // Ensure the slider reaches 100% visually
                elapsedTime = 0f;  // Reset elapsed time to simulate a smooth transition
                yield return new WaitForSeconds(0.5f);  // Add a small delay before activating the scene (0.5 seconds)
                operation.allowSceneActivation = true;  // Activate the scene after the delay
            }

            yield return null;  // Wait for the next frame
        }
    }

    // Custom function to apply the loading curve (non-linear progression)
    private float ApplyCustomLoadingCurve(float progress)
    {
        // Use the custom AnimationCurve to evaluate the current progress
        return loadingCurve.Evaluate(progress);  // Apply the curve to the progress
    }
}

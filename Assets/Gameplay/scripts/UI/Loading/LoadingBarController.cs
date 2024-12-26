using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBarController : MonoBehaviour
{
    [SerializeField]
    private Slider loadingSlider;
    [SerializeField]
    private TMP_Text loadingText;

    // This method updates the loading bar and text based on the progress (0 to 1)
    public void UpdateLoadingBar(float progress)
    {
        loadingSlider.value = progress;  // Update the slider value based on the progress
        loadingText.text = (progress * 100).ToString("F0") + "%";  // Update the text to show the percentage
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonTransparency : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image buttonImage;
    private Color originalColor;

    void Start()
    {
        // Get the Image component attached to the button
        buttonImage = GetComponent<Image>();

        // Store the original color of the button
        originalColor = buttonImage.color;

        SetButtonTransparency(0f);
    }

    // When the cursor enters the button area
    public void OnPointerEnter(PointerEventData eventData)
    {
        SetButtonTransparency(0.5f);  // Set 50% transparency (alpha = 0.5)
    }

    // When the cursor exits the button area
    public void OnPointerExit(PointerEventData eventData)
    {
        SetButtonTransparency(0f);  // Set 100% transparency (alpha = 0)
    }

    // Helper function to set the button transparency by adjusting the alpha value
    private void SetButtonTransparency(float alpha)
    {
        // Adjust only the alpha value of the button's original color
        Color newColor = originalColor;
        newColor.a = alpha;

        // Apply the new color with modified transparency to the button
        buttonImage.color = newColor;
    }
}

using UnityEngine;

public class GameUIHandler : MonoBehaviour
{
    [SerializeField]
    private Canvas canvasToToggle;

    void Update()
    {
        // Check if the Tab key is being held down
        if (Input.GetKey(KeyCode.Tab))
        {
            // Activate the Canvas
            canvasToToggle.gameObject.SetActive(true);
        }
        else
        {
            // Deactivate the Canvas when the Tab key is released
            canvasToToggle.gameObject.SetActive(false);
        }
    }
}

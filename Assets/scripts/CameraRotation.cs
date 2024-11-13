using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour
{
    [SerializeField]
    private Transform target; // The character the camera follows
    [SerializeField]
    private float rotationSpeed = 0.5f; // Slower rotation speed for smoother control

    private float currentAngleY = 0f;

    private void LateUpdate()
    {
        // Get mouse input for camera rotation
        float mouseX = Mouse.current.delta.x.ReadValue() * rotationSpeed;

        // Update rotation angle on the Y axis
        currentAngleY += mouseX;

        // Create a rotation based on the updated angle
        Quaternion rotation = Quaternion.Euler(0, currentAngleY, 0);

        // Calculate new position with an offset and keep the camera at the same distance
        Vector3 offset = new Vector3(0, 5, -10); // Adjust the offset as needed
        Vector3 targetPosition = target.position + rotation * offset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f); // Smooth camera movement

        // Make the camera look at the character
        transform.LookAt(target.position);
    }
}

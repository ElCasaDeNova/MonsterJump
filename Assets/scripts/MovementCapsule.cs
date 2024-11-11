using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MovementControls : MonoBehaviour
{
    private Rigidbody rigidBody;
    private InputAction moveAction;
    private InputAction jumpAction;
    private bool isGrounded;

    [SerializeField]
    private float speed = 6f;
    [SerializeField]
    private float jumpForce = 6f;
    [SerializeField]
    private Transform groundCheck;

    private LayerMask groundLayerMask;

    private Camera mainCamera;  // Reference to the main camera
    private Vector3 cameraOffset = new Vector3(0, 5, -10); // Offset for camera position

    private Controls controls;

    private void Awake()
    {
        controls = new Controls();
        rigidBody = GetComponent<Rigidbody>();
        groundLayerMask = LayerMask.GetMask("Ground");
        mainCamera = Camera.main;  // Get the main camera reference
    }

    private void OnEnable()
    {
        moveAction = controls.Player.Move;
        moveAction.Enable();

        jumpAction = controls.Player.Jump;
        jumpAction.performed += OnJump;
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.performed -= OnJump;
        jumpAction.Disable();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        // Check if the player is grounded
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, 0.05f, groundLayerMask);

        // Get movement direction from input
        Vector3 moveDir = moveAction.ReadValue<Vector2>();
        Vector3 vel = rigidBody.velocity;
        vel.x = speed * moveDir.x;
        vel.z = speed * moveDir.y;
        rigidBody.velocity = vel;

        // Handle the character's rotation
        if (moveDir.sqrMagnitude > 0.1f) // Avoid small movements
        {
            // Determine the direction to look towards
            Vector3 targetDirection = new Vector3(moveDir.x, 0, moveDir.y);

            // Rotate the character smoothly
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // Move the camera based on the character's position
        Vector3 targetCameraPosition = transform.position + cameraOffset;
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetCameraPosition, Time.deltaTime * 5f);

        // The camera should always look at the player (optional)
        mainCamera.transform.LookAt(transform.position + Vector3.up);
    }
}

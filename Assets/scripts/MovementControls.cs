using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MovementControls : MonoBehaviour
{
    private Rigidbody rigidBody;
    private InputAction moveAction;
    private InputAction jumpAction;
    private bool isGrounded;
    private bool canJump;

    [SerializeField]
    private float speed = 6f;
    [SerializeField]
    private float jumpForce = 6f;
    [SerializeField]
    private float playerRotationSpeed = 0.15f;
    [SerializeField]
    private Transform groundCheck;

    private LayerMask groundLayerMask;

    [SerializeField]
    private Camera mainCamera;  // Reference to the main camera
    [SerializeField]
    private Vector3 cameraOffset = new Vector3(0, 5, -10); // Offset for camera position
    [SerializeField]
    private float cameraRotationSpeed = 0.25f; // Speed for camera rotation around the player
    private float currentAngleY = 0f; // Current camera angle on the Y axis

    [SerializeField]
    private float coyoteTime = 0.2f;  // Time allowed after leaving the ground to perform a jump
    private float coyoteTimeCounter;

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
        if (isGrounded || canJump)
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z); // Reset the vertical velocity
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false; // Reset jump ability
        }
    }

    private void FixedUpdate()
    {
        // Check if the player is grounded
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, 0.1f, groundLayerMask);

        if (isGrounded)
        {
            // If grounded, reset coyote time
            coyoteTimeCounter = coyoteTime;
            canJump = true; // Allow jumping again when grounded
        }
        else
        {
            // Decrease coyote time if not grounded
            if (coyoteTimeCounter > 0f)
                coyoteTimeCounter -= Time.deltaTime;
            else
                canJump = false; // No more coyote time, can't jump
        }

        // Get movement direction from input
        Vector3 moveDir = moveAction.ReadValue<Vector2>();
        Vector3 vel = rigidBody.velocity;

        // Move character based on input direction, considering camera's orientation
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        forward.y = 0;  // Keep the forward direction flat
        right.y = 0;    // Keep the right direction flat
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * moveDir.y + right * moveDir.x).normalized;

        vel.x = speed * moveDirection.x;
        vel.z = speed * moveDirection.z;
        rigidBody.velocity = new Vector3(vel.x, rigidBody.velocity.y, vel.z); // Keep the y-velocity unchanged for gravity

        // Rotate the character towards movement direction
        if (moveDir.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rigidBody.MoveRotation(Quaternion.Slerp(rigidBody.rotation, targetRotation, playerRotationSpeed));
        }
    }

    private void Update()
    {
        // Handle camera rotation based on mouse movement (around Y axis only)
        HandleCameraRotation();
    }

    private void HandleCameraRotation()
    {
        // Get mouse input for camera rotation
        float mouseX = Mouse.current.delta.x.ReadValue() * cameraRotationSpeed;

        // Update rotation angle on the Y axis (left-right rotation)
        currentAngleY += mouseX;

        // Create a rotation based on the updated Y angle
        Quaternion rotation = Quaternion.Euler(0, currentAngleY, 0);

        // Update the camera position with respect to the character
        Vector3 offset = rotation * new Vector3(0, 5, -10); // Adjust this offset as needed (height and distance from character)
        mainCamera.transform.position = transform.position + offset;

        // Make the camera look at the player
        mainCamera.transform.LookAt(transform.position + Vector3.up);
    }
}

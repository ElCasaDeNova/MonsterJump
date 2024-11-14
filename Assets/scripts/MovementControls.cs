using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MovementControls : MonoBehaviour
{
    private Rigidbody rigidBody;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;  // Added for sprint functionality
    private bool isGrounded;
    private bool canJump;
    private bool isSprinting;  // Tracks if sprinting is active

    [SerializeField]
    private float speed = 6f;
    [SerializeField]
    private float sprintSpeed = 10f; // Speed when sprinting
    [SerializeField]
    private float jumpForce = 6f;
    [SerializeField]
    private float playerRotationSpeed = 0.15f;
    [SerializeField]
    private Transform groundCheck;

    private LayerMask groundLayerMask;

    [SerializeField]
    private Camera mainCamera;
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

        // Enable sprint action
        sprintAction = controls.Player.Sprint;
        sprintAction.performed += OnSprint; // Register sprint callback
        sprintAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.performed -= OnJump;
        jumpAction.Disable();

        // Disable sprint action
        sprintAction.performed -= OnSprint; // Unregister sprint callback
        sprintAction.Disable();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded || canJump)
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z); // Reset vertical velocity
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false; // Reset jump ability
        }
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        // Enable sprint only if grounded
        if (isGrounded)
        {
            isSprinting = true;
        }
    }

    private void FixedUpdate()
    {
        // Check if the player is grounded
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, 0.1f, groundLayerMask);

        if (isGrounded)
        {
            // Reset coyote time if grounded
            coyoteTimeCounter = coyoteTime;
            canJump = true; // Allow jumping when grounded

            // Reset sprint status if grounded
            if (sprintAction.ReadValue<float>() == 0)
            {
                isSprinting = false; // Sprint stops when the sprint key is released on the ground
            }
        }
        else
        {
            // Decrease coyote time if not grounded
            if (coyoteTimeCounter > 0f)
                coyoteTimeCounter -= Time.deltaTime;
            else
                canJump = false; // Out of coyote time, cannot jump
        }

        // Get movement direction from input
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 vel = rigidBody.velocity;

        // Use sprint speed if sprint is active, otherwise use normal speed
        float currentSpeed = isSprinting ? sprintSpeed : speed;

        // Move character based on input direction, considering camera's orientation
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        forward.y = 0;  // Flatten the forward vector
        right.y = 0;    // Flatten the right vector
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * moveInput.y + right * moveInput.x).normalized;

        vel.x = currentSpeed * moveDirection.x;
        vel.z = currentSpeed * moveDirection.z;
        rigidBody.velocity = new Vector3(vel.x, rigidBody.velocity.y, vel.z); // Keep the y-velocity unchanged for gravity

        // Rotate the character towards the movement direction
        if (moveInput.sqrMagnitude > 0.1f)
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

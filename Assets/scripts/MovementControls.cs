using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MovementControls : MonoBehaviour
{
    private Rigidbody rigidBody;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction lookAction;
    private bool isGrounded;
    private bool canJump;
    private bool isSprinting;

    [SerializeField]
    private float speed = 6f; // Normal walking speed
    [SerializeField]
    private float sprintSpeed = 10f; // Speed when sprinting
    [SerializeField]
    private float jumpForce = 6f; // Force applied for jumping
    [SerializeField]
    private float playerRotationSpeed = 0.15f; // Speed for rotating the player to face the movement direction
    [SerializeField]
    private Transform groundCheck; // Transform used to check if the player is grounded

    private LayerMask groundLayerMask; // Layer mask to identify what is considered ground
    private PhysicMaterial noFrictionMaterial; // Material to reduce friction and prevent sticking to walls

    [SerializeField]
    private Camera mainCamera; // Reference to the main camera
    [SerializeField]
    private Vector3 cameraOffset = new Vector3(0, 5, -10); // Camera position offset
    [SerializeField]
    private float cameraRotationSpeed = 0.25f; // Speed of camera rotation
    private float currentAngleY = 0f; // Current rotation angle of the camera around the Y-axis

    [SerializeField]
    private float coyoteTime = 0.2f; // Time window after leaving the ground during which a jump can still be triggered
    private float coyoteTimeCounter; // Counter to keep track of coyote time

    [SerializeField]
    private Animator animator; // To modify parameters of walking and jump animations

    private Controls controls; // Input system controls reference

    private bool canDoubleJump; // Flag for double jump

    private bool wasGrounded = false;

    [SerializeField]
    private float fallSpeedIncrease = 1f;

    private void Awake()
    {
        // Get private variables
        controls = new Controls();
        rigidBody = GetComponent<Rigidbody>();
        groundLayerMask = LayerMask.GetMask("Ground");
        mainCamera = Camera.main;

        // Create a physic material with no friction to prevent sticking to walls
        noFrictionMaterial = new PhysicMaterial
        {
            dynamicFriction = 0,
            staticFriction = 0,
            frictionCombine = PhysicMaterialCombine.Minimum
        };
        GetComponent<Collider>().material = noFrictionMaterial;
    }

    // Called when GameObject is Activate
    private void OnEnable()
    {
        // Enable movement, jump, and sprint actions
        moveAction = controls.Player.Move;
        moveAction.Enable();

        jumpAction = controls.Player.Jump;
        jumpAction.performed += OnJump;
        jumpAction.Enable();

        sprintAction = controls.Player.Sprint;
        sprintAction.performed += OnSprint;
        sprintAction.Enable();

        lookAction = controls.Player.Look;
        lookAction.Enable();
    }

    // Called when GameObject is DeActivate
    private void OnDisable()
    {
        // Disable movement, jump, and sprint actions
        moveAction.Disable();
        jumpAction.performed -= OnJump;
        jumpAction.Disable();

        sprintAction.performed -= OnSprint;
        sprintAction.Disable();

        lookAction.Disable();
    }

    // Called when Jump button is pressed
    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded || canJump)
        {
            Jump();
        }
        else if (canDoubleJump) // Double jump allowed
        {
            DoubleJump();
        }
    }

    private void Jump()
    {
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z); // Reset vertical velocity
        rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        canJump = false; // Single jump used
        canDoubleJump = true; // Enable double jump

        // Launch Animation
        animator.SetBool("isGrounded", false);
        animator.SetBool("isJumping", true);
    }

    private void DoubleJump()
    {
        // Launch Animation
        animator.SetBool("isDoubleJumping", true);

        rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z); // Reset vertical velocity
        rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        canDoubleJump = false; // Disable double jump after use
    }

    // Called when Sprint button is pressed
    private void OnSprint(InputAction.CallbackContext context)
    {
        // Enable sprint only when the player is on the ground
        if (isGrounded)
        {
            isSprinting = true;
        }
    }

    // Called at Fixed Frequences (50 times per second)
    private void FixedUpdate()
    {
        // Check if the player is on the ground
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, 0.1f, groundLayerMask);

        // Deal with Coyote Jump (Allow to jump a small time after falling)
        if (isGrounded)
        {
            if (!wasGrounded) // if character was Jumping
            {
                OnLanding();
            }
            ResetCoyoteJump(); // Reset Coyote Jump Countdown
        }
        else
        {
            // Start Coyote Jump Countdown
            StartCoyoteTime();
        }

        // Help to know if character was Falling or Grounded
        wasGrounded = isGrounded;

        // Add Super Jump Gravity
        bool isFalling = rigidBody.velocity.y < 0;
        if (isFalling)
        {
            AddFallingForce();
        }

        // Set the speed (bigger if sprinting)
        float currentSpeed = isSprinting ? sprintSpeed : speed;

        // Read movement inputs
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        // Calculate Velocity
        Vector3 vel = rigidBody.velocity;

        // Calculte Movement Direction
        Vector3 moveDirection = CalculateMovementDirection(moveInput);

        ApplyMovement(vel, currentSpeed, moveDirection);

        // Update animator parameters for walking animations
        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);

        // Rotate only if the character is not facing the same direction as the camera
        bool playerIsMoving = moveInput.sqrMagnitude > 0.1f;
        if (playerIsMoving)
        {
            RotateCharacter();
        }
    }

    private void AddFallingForce()
    {
        rigidBody.velocity += Vector3.down * fallSpeedIncrease * Time.deltaTime;
    }

    private void OnLanding()
    {
        ResetAnimator();
    }

    private void ResetCoyoteJump()
    {
        // Reset coyote time and allow jumping if the player is on the ground
        coyoteTimeCounter = coyoteTime;
        canJump = true;

        // If the player releases the sprint button, stop running
        if (sprintAction.ReadValue<float>() == 0)
        {
            isSprinting = false;
        }

    }

    private void ResetAnimator()
    {
        animator.SetBool("isJumping", false);
        animator.SetBool("isDoubleJumping", false);
        animator.SetBool("isGrounded", true);
    }

    private void StartCoyoteTime()
    {
        // Decrease the coyote time counter if the player is in the air
        if (coyoteTimeCounter > 0f)
            coyoteTimeCounter -= Time.deltaTime;
        else
            canJump = false; // Cannot jump if coyote time has expired
    }

    private Vector3 CalculateMovementDirection(Vector2 moveInput)
    {
        // Calculate movement direction based on the camera
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        forward.y = 0; // Ignore verticality
        right.y = 0; // Ignore verticality
        forward.Normalize(); // Normalize to avoid diagonal movement
        right.Normalize(); // Normalize to avoid diagonal movement

        return forward * moveInput.y + right * moveInput.x;
    }

    private void ApplyMovement(Vector3 vel, float currentSpeed, Vector3 moveDirection)
    {
        // Apply movement direction locally
        vel.x = currentSpeed * moveDirection.x;
        vel.z = currentSpeed * moveDirection.z;

        CheckWallCollision(moveDirection, vel);
    }

    private void RotateCharacter()
    {
        // Calculate the forward direction of the camera and the character
        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0; // Ignore vertical component
        cameraForward.Normalize(); // Normalize for accurate comparison

        Vector3 characterForward = transform.forward;
        characterForward.y = 0; // Ignore vertical component
        characterForward.Normalize(); // Normalize for accurate comparison

        // Check if the character's forward direction is significantly different from the camera's forward direction
        float angleBetweenCameraAndCharacter = Vector3.Angle(characterForward, cameraForward);

        // Rotate only if the angle exceeds a small threshold (e.g., 5 degrees) to ensure they are not aligned
        if (angleBetweenCameraAndCharacter > 5f)
        {
            // Rotate the character to face the direction of the camera
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            rigidBody.MoveRotation(Quaternion.Slerp(rigidBody.rotation, targetRotation, playerRotationSpeed));
        }
    }

    private void CheckWallCollision(Vector3 moveDirection, Vector3 vel)
    {
        // Check for collisions (to avoid sticking to walls)
        bool isAgainstWall = Physics.Raycast(transform.position, moveDirection, 0.5f);

        // Apply speed to the Rigidbody if not touching a wall
        if (!isAgainstWall)
        {
            rigidBody.velocity = new Vector3(vel.x, rigidBody.velocity.y, vel.z);
        }
    }

    private void Update()
    {
        // Handle camera rotation logic
        HandleCameraRotation();
    }

    private void HandleCameraRotation()
    {
        // Get the delta value from the lookAction (mouse or controller movement)
        Vector2 lookDelta = lookAction.ReadValue<Vector2>();

        // Apply horizontal movement (X axis) for camera rotation
        float mouseX = lookDelta.x * cameraRotationSpeed;

        // Update the camera's rotation angle
        currentAngleY += mouseX;
        Quaternion rotation = Quaternion.Euler(0, currentAngleY, 0);

        // Calculate the camera's offset based on the rotation
        Vector3 offset = rotation * cameraOffset;

        // Update the camera's position
        mainCamera.transform.position = transform.position + offset;

        // Make the camera look at the player (with a slight upward offset)
        mainCamera.transform.LookAt(transform.position + Vector3.up);
    }
}

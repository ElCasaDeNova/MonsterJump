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

    [SerializeField]
    private float inputSmoothSpeed = 5f; //Smooth input speed
    private float smoothedInputX = 0f;
    private float smoothedInputY = 0f;

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
    private float smoothRotationSpeed = 0.1f;

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

    [SerializeField]
    private float animationSprintBoost = 1.1f;
    private float animatorSpeed;

    private float inputDeadZone = 0.01f;

    private CharacterSoundEffect characterSoundEffect;

    [SerializeField]
    private float stepHeight = 0.5f; // Maximum Height for a step
    [SerializeField]
    private float stepCheckDistance = 0.6f; // Distance of Step detection

    [SerializeField]
    private float cameraCollisionRadius = 0.5f; // Radius for camera collision detection
    [SerializeField]
    private LayerMask cameraCollisionLayer; // Layer mask for camera collision detection (e.g., Ground layer)

    [SerializeField]
    private Vector3 cameraVelocity = Vector3.zero;


    private void Awake()
    {
        // Get private variables
        controls = new Controls();
        rigidBody = GetComponent<Rigidbody>();
        groundLayerMask = LayerMask.GetMask("Ground");
        mainCamera = Camera.main;
        characterSoundEffect = GetComponent<CharacterSoundEffect>();

        // Create a physic material with no friction to prevent sticking to walls
        noFrictionMaterial = new PhysicMaterial
        {
            dynamicFriction = 0,
            staticFriction = 0,
            frictionCombine = PhysicMaterialCombine.Minimum
        };
        GetComponent<Collider>().material = noFrictionMaterial;

        animatorSpeed = animator.speed;
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

    public void DoubleJump()
    {
        characterSoundEffect.PlayJumpVoice();

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
            animator.SetBool("isSprinting", true);
            animatorSpeed = animator.speed;
            animator.speed = animator.speed * animationSprintBoost;
        }
    }

    // Called at Fixed Frequences (50 times per second)
    private void FixedUpdate()
    {
        HandleStepClimb();

        // Check if the player is on the ground
        isGrounded = CheckIfGrounded();

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
            animator.SetBool("isGrounded", false);
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

        // Smooth Input
        smoothedInputX = Mathf.Lerp(smoothedInputX, moveInput.x, Time.deltaTime * inputSmoothSpeed);
        smoothedInputY = Mathf.Lerp(smoothedInputY, moveInput.y, Time.deltaTime * inputSmoothSpeed);

        // Vérifie if not moving
        if (Mathf.Abs(smoothedInputX) < inputDeadZone) smoothedInputX = 0f;
        if (Mathf.Abs(smoothedInputY) < inputDeadZone) smoothedInputY = 0f;

        animator.SetFloat("InputX", smoothedInputX);
        animator.SetFloat("InputY", smoothedInputY);

        // Calculate Movement Direction
        Vector3 moveDirection = CalculateMovementDirection(new Vector2(smoothedInputX, smoothedInputY));

        // Calculate velocity
        Vector3 vel = rigidBody.velocity;

        ApplyMovement(vel, currentSpeed, moveDirection);

        // Rotate only if the character is not facing the same direction as the camera
        bool playerIsMoving = moveInput.sqrMagnitude > 0.1f;
        if (playerIsMoving)
        {
            RotateCharacter();
        }
    }

    private Transform currentPlatform = null;
    private bool CheckIfGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(groundCheck.position, Vector3.down, out hit, 0.5f, groundLayerMask))
        {
            // Check if the ground is an unstable platform
            if (hit.collider.CompareTag("UnstablePlatform"))
            {
                // If the platform is unstable, we add a bit of tolerance
                Vector3 platformNormal = hit.normal; // Normal of the platform
                float distanceToPlatform = hit.distance; // Distance from the platform

                // Check if we are close enough to the platform to be considered grounded
                if (distanceToPlatform < 1.25f && Mathf.Abs(platformNormal.y) > 0.15f)
                {
                    // Check if the platform is moving vertically (i.e., if the character is moving up or down with it)
                    Vector3 platformVelocity = hit.rigidbody != null ? hit.rigidbody.velocity : Vector3.zero;

                    // Check if the player's vertical velocity is close to the platform's vertical velocity
                    // This ensures the player is moving with the platform
                    if (Mathf.Abs(rigidBody.velocity.y - platformVelocity.y) < 0.1f)
                    {
                        // If the player's vertical velocity is close to the platform's velocity, we consider the player
                        // to be moving up or down with the platform
                        return true;
                    }
                }
            }
            else if (hit.collider.CompareTag("MovingPlatform"))
            {
                Vector3 platformVelocity = hit.rigidbody != null ? hit.rigidbody.velocity : Vector3.zero;

                // Verify if player and platform have same velocity
                if (Mathf.Abs(rigidBody.velocity.y - platformVelocity.y) < 0.1f)
                {
                    currentPlatform = hit.transform; // attach player to platform
                    return true;
                }
            }
            else
            {
                currentPlatform = null; // Detached
                return true;
            }

            // If it's another type of stable ground, we return true as normal
            return true;
        }

        // If no ground is found, return false
        return false;
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
            animator.SetBool("isSprinting", false);
            animator.speed = animatorSpeed;
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

    private void HandleStepClimb()
    {
        RaycastHit hitLower;
        // Lancer un rayon légèrement au-dessus de la position du joueur pour détecter les obstacles devant lui
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, transform.forward, out hitLower, stepCheckDistance, groundLayerMask))
        {
            // Calculer la hauteur de l'obstacle devant le joueur
            float obstacleHeight = hitLower.point.y - transform.position.y;

            // Vérifier si l'obstacle est dans la plage de hauteur des marches
            if (obstacleHeight > 0.1f && obstacleHeight <= stepHeight)
            {
                RaycastHit hitUpper;
                // Lancer un second rayon pour vérifier s'il y a de l'espace libre au-dessus de l'obstacle
                bool isSpaceAboveClear = !Physics.Raycast(transform.position + Vector3.up * (stepHeight + 0.1f), transform.forward, out hitUpper, stepCheckDistance, groundLayerMask);

                if (isSpaceAboveClear)
                {
                    // Ajuster la position du joueur pour monter l'obstacle
                    Vector3 newPosition = transform.position;
                    newPosition.y += obstacleHeight;  // Déplacer le joueur vers le haut de la hauteur de l'obstacle
                    transform.position = newPosition;  // Mettre à jour la position du joueur
                }
                else
                {
                    Debug.Log("Step climb blocked by obstacle above: " + hitUpper.collider.name);
                }
            }
        }
    }


    private void Update()
    {
        // Handle camera rotation logic
        HandleCameraRotation();
    }

    private void HandleCameraRotation()
    {
        // Get the mouse delta for rotation (X-axis)
        Vector2 lookDelta = lookAction.ReadValue<Vector2>();

        // Apply mouse movement to the X-axis rotation
        float mouseX = lookDelta.x * cameraRotationSpeed;

        // Update the Y-axis rotation angle
        currentAngleY += mouseX;

        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.Euler(0, currentAngleY, 0);

        // Apply the camera rotation
        mainCamera.transform.rotation = targetRotation;

        // Calculate the target camera position with the offset
        Vector3 offset = targetRotation * cameraOffset;
        Vector3 desiredCameraPosition = transform.position + offset;

        // Check for collisions with the ground and adjust the camera position
        RaycastHit hit;
        if (Physics.Raycast(transform.position, offset, out hit, offset.magnitude, cameraCollisionLayer))
        {
            // If a collision is detected, adjust the camera position
            desiredCameraPosition = hit.point - offset.normalized * cameraCollisionRadius;
        }

        // Move the camera to the desired position without passing through obstacles
        mainCamera.transform.position = desiredCameraPosition;

        // Make sure the camera always looks at the player
        mainCamera.transform.LookAt(transform.position + Vector3.up);
    }


    public void ResetCameraPosition(float newAngleY)
    {
        currentAngleY = newAngleY;
        mainCamera.transform.position = transform.position + cameraOffset;
        Quaternion targetRotation = Quaternion.Euler(0, currentAngleY, 0);
        mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, targetRotation, smoothRotationSpeed);
    }



}

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MovementControls : MonoBehaviour
{
    private Rigidbody rigidBody;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
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
    private Animator animator; // To Modify Parameters of Walking and Jump Animations

    private Controls controls; // Input system controls reference

    private bool canDoubleJump; // Flag for double jump

    private void Awake()
    {
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
    }

    private void OnDisable()
    {
        // Disable movement, jump, and sprint actions
        moveAction.Disable();
        jumpAction.performed -= OnJump;
        jumpAction.Disable();

        sprintAction.performed -= OnSprint;
        sprintAction.Disable();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded || canJump)
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z); // Reset vertical velocity
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false; // Single jump used
            canDoubleJump = true; // Enable double jump
        }
        else if (canDoubleJump) // Double jump allowed
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z); // Reset vertical velocity
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canDoubleJump = false; // Disable double jump after use
        }
    }


    private void OnSprint(InputAction.CallbackContext context)
    {
        // Enable sprint only when the player is on the ground
        if (isGrounded)
        {
            isSprinting = true;
        }
    }

    private void FixedUpdate()
    {
        // Vérifier si le joueur est au sol
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, 0.1f, groundLayerMask);

        if (isGrounded)
        {
            // Réinitialisation du coyote time et possibilité de sauter si le joueur est au sol
            coyoteTimeCounter = coyoteTime;
            canJump = true;

            // Si le joueur relâche la touche sprint, on arrête de courir
            if (sprintAction.ReadValue<float>() == 0)
            {
                isSprinting = false;
            }
        }
        else
        {
            // Diminution du compteur de coyote time si le joueur est dans les airs
            if (coyoteTimeCounter > 0f)
                coyoteTimeCounter -= Time.deltaTime;
            else
                canJump = false; // On ne peut plus sauter si le coyote time est écoulé
        }

        // Lecture des entrées de mouvement
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 vel = rigidBody.velocity;

        // Définir la vitesse en fonction du sprint
        float currentSpeed = isSprinting ? sprintSpeed : speed;

        // Calcul de la direction du mouvement en fonction de la caméra
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        forward.y = 0; // On annule la composante verticale
        right.y = 0; // On annule la composante verticale
        forward.Normalize(); // Normalisation pour éviter les déplacements diagonaux
        right.Normalize(); // Normalisation pour éviter les déplacements diagonaux

        // Calcul du mouvement en fonction de la direction de la caméra
        Vector3 moveDirection = forward * moveInput.y + right * moveInput.x;

        // Appliquer la direction du mouvement local
        vel.x = currentSpeed * moveDirection.x;
        vel.z = currentSpeed * moveDirection.z;

        // Vérification des collisions (pour éviter de s'accrocher aux murs)
        bool isAgainstWall = Physics.Raycast(transform.position, moveDirection, 0.5f);

        // Appliquer la vitesse au Rigidbody si on ne touche pas un mur
        if (!isAgainstWall)
        {
            rigidBody.velocity = new Vector3(vel.x, rigidBody.velocity.y, vel.z);
        }

        // Mettre à jour les paramètres de l'Animator pour les animations
        // Ce qui compte ici, c'est le mouvement local du personnage par rapport à sa propre orientation
        animator.SetFloat("InputX", moveInput.x); // Mouvement horizontal relatif à l'orientation du personnage
        animator.SetFloat("InputZ", moveInput.y); // Mouvement avant/arrière relatif à l'orientation du personnage

        // Faire tourner le personnage en fonction de la direction du mouvement
        if (moveInput.sqrMagnitude > 0.1f) // Si on bouge
        {
            // Faire tourner le personnage vers la direction du mouvement (local)
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rigidBody.MoveRotation(Quaternion.Slerp(rigidBody.rotation, targetRotation, playerRotationSpeed));
        }
    }



    private void Update()
    {
        // Handle camera rotation logic
        HandleCameraRotation();
    }

    private void HandleCameraRotation()
    {
        // Get mouse input for rotating the camera
        float mouseX = Mouse.current.delta.x.ReadValue() * cameraRotationSpeed;

        // Update the camera's rotation angle
        currentAngleY += mouseX;
        Quaternion rotation = Quaternion.Euler(0, currentAngleY, 0);

        // Set the camera position based on the player's position and rotation
        Vector3 offset = rotation * cameraOffset;
        mainCamera.transform.position = transform.position + offset;

        // Make the camera look at the player
        mainCamera.transform.LookAt(transform.position + Vector3.up);
    }
}

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MovementControls : MonoBehaviour
{
    private Rigidbody rigidBody;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private bool isGrounded;

    [SerializeField]
    private float speed = 6f;
    [SerializeField]
    private float jumpForce = 6f;

    [SerializeField]
    private Transform groundCheck;

    private LayerMask groundLayerMask;

    private Controls controls;

    private void Awake()
    {
        // Initialize Controls 
        controls = new Controls();

        rigidBody = GetComponent<Rigidbody>();

        groundLayerMask = LayerMask.GetMask("Ground");
    }

    private void OnEnable()
    {
        // Activate Action Maps
        moveAction = controls.Player.Move;
        moveAction.Enable();

        lookAction = controls.Player.Look;
        lookAction.Enable();

        jumpAction = controls.Player.Jump;
        // If Button is Pressed then OnJump is called
        jumpAction.performed += OnJump;
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        // DeActivate Action Map
        moveAction.Disable();
        lookAction.Disable();

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
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, 0.05f, groundLayerMask);

        Vector3 moveDir = moveAction.ReadValue<Vector2>();
        Vector3 vel = rigidBody.velocity;
        vel.x = speed * moveDir.x;
        vel.z = speed * moveDir.y;
        rigidBody.velocity = vel;
        // Debug.Log($"move : {moveDir}");

        Vector3 lookDir = lookAction.ReadValue<Vector2>();
        if (lookDir.sqrMagnitude > 0.1f) // Avoid small movement
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(lookDir.x, 0, lookDir.y));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);  // Adapt speed on rotation
        }

        // Debug.Log($"look : {lookDir}");
    }
}

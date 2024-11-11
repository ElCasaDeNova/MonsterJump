using UnityEngine;
using UnityEngine.InputSystem;

public class MovementCapsule : MonoBehaviour
{
    public float speed;

    private Controls controls;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

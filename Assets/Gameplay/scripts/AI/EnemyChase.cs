using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    [SerializeField]
    private Transform player; // Reference to the player's transform
    [SerializeField]
    private float stopDistance = 1f;
    [SerializeField]
    private GameObject hand;
    private BoxCollider boxCollider; //BoxCollider of the Hand
    private Animator animator;
    private NavMeshAgent agent; // Reference to the NavMeshAgent component
    private float originalSpeed;



    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originalSpeed = agent.speed;
        boxCollider = hand.GetComponent<BoxCollider>();
        animator = agent.GetComponent<Animator>();
    }

    void Update()
    {
        // if Chase Mode is Active
        if (agent.isActiveAndEnabled)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
           
            //If Player is close, then stop and Punch Player
            if (distanceToPlayer <= stopDistance)
            {
                StopAndPunch();
            }
            else
            {
                ChasePlayer();
            }
        }


    }

    void StopAndPunch()
    {
        agent.speed = 0;
        animator.SetBool("isPunching", true);
    }

    void ChasePlayer()
    {
        agent.speed = originalSpeed;
        agent.destination = player.position;
        if (animator != null)
        {
            animator.SetBool("isPunching", false);
        }
    }

    public void ActivatePunchBoxCollider()
    {
        boxCollider.enabled = true;
    }

    public void DeactivatePunchBoxCollider()
    {
        boxCollider.enabled = false;
    }
}

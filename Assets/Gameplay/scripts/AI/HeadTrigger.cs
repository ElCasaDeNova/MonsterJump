using UnityEngine;
using UnityEngine.AI;

public class HeadTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject agent;
    [SerializeField]
    private float killValue = 1f;
    private MovementControls movementControls;
    private CharacterSoundEffect characterSoundEffect;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private BlinkController blinkController;
    private BoxCollider bodyBoxCollider;
    private BoxCollider headBoxCollider;

    private void Start()
    {
        movementControls = player.GetComponent<MovementControls>();
        characterSoundEffect = agent.GetComponent<CharacterSoundEffect>();
        animator = agent.GetComponent<Animator>();
        navMeshAgent = agent.GetComponent<NavMeshAgent>();
        blinkController = agent.GetComponent<BlinkController>();
        bodyBoxCollider = agent.GetComponent<BoxCollider>();
        headBoxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // Add points to the score
            Score.killPoints+= killValue;

            // DeActivate Box Collider for the body
            if (bodyBoxCollider != null)
            {
                bodyBoxCollider.enabled = false;
            }

            // DeActivate Box Collider for the head
            if (headBoxCollider != null)
            {
                headBoxCollider.enabled = false;
            }

            movementControls.DoubleJump();

            //activate animation for death
            animator.SetBool("isDead", true);

            // Enemy stop chasing
            navMeshAgent.speed = 0;
            navMeshAgent.enabled = false;

            // enemy blinks before disappearing
            blinkController.Blink();
        }
    }



}

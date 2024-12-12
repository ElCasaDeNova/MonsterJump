using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class HeadTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject agent;
    private MovementControls movementControls;
    private CharacterSoundEffect characterSoundEffect;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private BlinkController blinkController;

    private void Start()
    {
        movementControls = player.GetComponent<MovementControls>();
        characterSoundEffect = agent.GetComponent<CharacterSoundEffect>();
        animator = agent.GetComponent<Animator>();
        navMeshAgent = agent.GetComponent<NavMeshAgent>();
        blinkController = agent.GetComponent<BlinkController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // DeActivate Box Collider
            BoxCollider boxCollider = GetComponent<BoxCollider>();
            if (boxCollider != null)
            {
                boxCollider.enabled = false; // Désactiver le BoxCollider
            }

            // Check if the object entering has the tag "Player"
            if (other.CompareTag("Player"))
            {
                // Send the message "jump"
                Debug.Log("Enemy Touched on Head");

                movementControls.DoubleJump();

                //activate animation for death
                animator.SetBool("isDead", true);

                // Enemy stop chasing
                navMeshAgent.speed = 0;

                // enemy blinks before disappearing
                blinkController.Blink();
            }
        }
    }

    

}

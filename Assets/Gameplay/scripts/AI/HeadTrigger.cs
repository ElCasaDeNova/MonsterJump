using UnityEngine;

public class HeadTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject agent;
    [SerializeField]
    private FBIAgentPooler agentPooler;
    private MovementControls movementControls;
    private CharacterSoundEffect characterSoundEffect;
    private Animator animator;

    private void Start()
    {
        movementControls = player.GetComponent<MovementControls>();
        characterSoundEffect = agent.GetComponent<CharacterSoundEffect>();
        animator = agent.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // Send the message "jump"
            Debug.Log("Enemy Touched on Head");

            movementControls.DoubleJump();

            //TODO Enemy makes Sounds
            //characterSoundEffect.();

            //TODO activate animation for death
            //animator.SetBool("isDead",true);

            //TODO kill enemy after few seconds
            agentPooler.ReturnAgent(agent.gameObject);
        }
    }
}

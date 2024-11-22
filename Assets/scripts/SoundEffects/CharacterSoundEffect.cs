using UnityEngine;

public class CharacterSoundEffect : MonoBehaviour
{
    [SerializeField]
    private AudioSource stepSource;
    [SerializeField]
    private AudioSource voiceSource;

    [SerializeField]
    private AudioClip stepSound;
    [SerializeField]
    private AudioClip voiceSound;

    private float lastPlayTime = 0f;
    [SerializeField]
    private float minimumInterval = 0.2f;
    [SerializeField]
    private float minimumsprintInterval = 0.15f;
    [SerializeField]
    private Animator animator;

    void PlayFootsteps()
    {
        // verify character is on Floor
        if (animator.GetBool("isGrounded")) {
            // Control if character is sprinting
            if (animator.GetBool("isSprinting"))
            {
                // Control the maximum footstep frequency while sprinting
                if (Time.time - lastPlayTime >= minimumsprintInterval)
                {
                    if (stepSource != null && stepSound != null)
                    {
                        PlayStep();
                    }
                    else
                    {
                        Debug.LogWarning("AudioSource or AudioClip are not assigned");
                    }
                }

            }
            // Control the maximum footstep frequency
            else if (Time.time - lastPlayTime >= minimumInterval)
            {
                if (stepSource != null && stepSound != null)
                {
                    PlayStep();
                }
                else
                {
                    Debug.LogWarning("AudioSource or AudioClip are not assigned");
                }
            }
        }     
    }

    private void PlayStep()
    {
        float originalPitch = stepSource.pitch;
        stepSource.pitch = originalPitch * Random.Range(0.9f, 1.1f);
        stepSource.clip = stepSound;
        stepSource.Play();
        stepSource.pitch = originalPitch;

        lastPlayTime = Time.time;
    }

    void PlayJumpVoice()
    {
        if (voiceSource != null && voiceSound != null)
        {
            voiceSource.pitch = Random.Range(0.8f, 1.2f);
            voiceSource.clip = voiceSound;
            voiceSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip are not assigned");
        }
    }
}

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
    [SerializeField]
    private AudioClip landingSound;
    [SerializeField]
    private float stepVolume=1f;

    private float lastStepTime = 0f;
    [SerializeField]
    float originalStepPitch = 1f;
    [SerializeField]
    private float minimumInterval = 0.2f;
    [SerializeField]
    private float minimumSprintInterval = 0.15f;

    [SerializeField]
    float originalVoicePitch = 1f;

    [SerializeField]
    private Animator animator;

    private float lastLandingTime = 0f;
    [SerializeField]
    private float minimumLandingInterval = 0.15f;

    [SerializeField]
    private float landingVolumeReduction = 0.9f;

    void PlayFootsteps()
    {
        // verify character is on Floor
        if (animator.GetBool("isGrounded"))
        {
            // Control if character is sprinting
            if (animator.GetBool("isSprinting"))
            {
                // Control the maximum footstep frequency while sprinting
                if (Time.time - lastStepTime >= minimumSprintInterval)
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
            else if (Time.time - lastStepTime >= minimumInterval)
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
        float randomPitch = Random.Range(0.95f, 1.05f);
        stepSource.pitch = originalStepPitch * randomPitch;

        stepSource.volume = stepVolume;

        stepSource.clip = stepSound;
        stepSource.Play();

        lastStepTime = Time.time;
    }

    void PlayLandingSound()
    {
        stepSource.pitch = originalStepPitch;

        if (Time.time - lastLandingTime >= minimumLandingInterval)
        {
            if (stepSource != null && landingSound != null)
            {
                stepSource.clip = landingSound;

                float randomPitch = Random.Range(0.5f, 1.5f);
                stepSource.pitch = originalStepPitch * randomPitch;

                stepSource.volume = landingVolumeReduction;

                stepSource.Play();
            }
            else
            {
                Debug.LogWarning("AudioSource or AudioClip are not assigned");
            }
        }
    }

    public void PlayJumpVoice()
    {
        voiceSource.pitch = originalVoicePitch;

        if (voiceSource != null && voiceSound != null)
        {
            voiceSource.pitch = originalVoicePitch * Random.Range(0.95f, 1.05f);
            voiceSource.clip = voiceSound;
            voiceSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip are not assigned");
        }
    }
}

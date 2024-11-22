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

    void PlayFootsteps()
    {
        // Control the maximum footstep frequency
        if (Time.time - lastPlayTime >= minimumInterval)
        {
            if (stepSource != null && stepSound != null)
            {
                float originalPitch = stepSource.pitch;
                stepSource.pitch = originalPitch * Random.Range(0.9f, 1.1f);
                stepSource.clip = stepSound;
                stepSource.Play();
                stepSource.pitch = originalPitch;

                lastPlayTime = Time.time;
            }
            else
            {
                Debug.LogWarning("AudioSource or AudioClip are not assigned");
            }
        }
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

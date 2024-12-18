using System.Collections;
using UnityEngine;

public class PlayerHitHandler : MonoBehaviour
{
    [SerializeField]
    private Material normalMaterial;
    [SerializeField]
    private Material damagedMaterial;
    [SerializeField]
    private GameObject playerBody;
    [SerializeField]
    private float damageEffectDuration=1f;
    [SerializeField]
    private float UpForce = 1f;
    [SerializeField]
    private float BackForce = 10f;
    [SerializeField]
    private float MovementDuration = 0.5f;
    [SerializeField]
    private float originalVoicePitch = 1.5f;

    private CharacterSoundEffect characterSoundEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyDamageEffect(other);

            StartCoroutine(RestoreNormalStateAfterDelay());
        }
    }

    // Turn Character to Red and move him to the back
    private void ApplyDamageEffect(Collider player)
    {
        if (playerBody.TryGetComponent<Renderer>(out Renderer renderer))
        {
            renderer.material = damagedMaterial;
        }

        // Add Forces on Player when hit
        StartCoroutine(MoveBackwardAndUp(player));

        // Add Hit Noise
        characterSoundEffect = player.GetComponent<CharacterSoundEffect>();
        characterSoundEffect.PlayHitSound(originalVoicePitch);

        //TODO Player Health (component) -1
    }

    private IEnumerator MoveBackwardAndUp(Collider player)
    {
        // Calculate Player Direction
        Vector3 initialPosition = player.transform.position;
        Vector3 targetPosition = player.transform.position
            - player.transform.forward * BackForce // Vers l'arrière
            + player.transform.up * UpForce;      // Vers le haut

        float elapsedTime = 0f;

        while (elapsedTime < MovementDuration)
        {
            // Interpolation between  initial position and target
            player.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / MovementDuration);

            // Increment time
            elapsedTime += Time.deltaTime;

            // Wait next frame
            yield return null;
        }

        // Verify player is on target
        player.transform.position = targetPosition;
    }

    // Gives to the character his original color after few seconds
    private IEnumerator RestoreNormalStateAfterDelay()
    {
        yield return new WaitForSeconds(damageEffectDuration);

        if (playerBody.TryGetComponent<Renderer>(out Renderer renderer))
        {
            renderer.material = normalMaterial;
        }
    }
}

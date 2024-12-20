using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private float maximumHealth = 3f;
    [SerializeField]
    private RespawnHandler respawnHandler;
    [SerializeField]
    private Material normalMaterial;
    [SerializeField]
    private Material damagedMaterial;
    [SerializeField]
    private GameObject playerBody;
    [SerializeField]
    private float damageEffectDuration = 1f;
    [SerializeField]
    private float MovementDuration = 0.5f;
    [SerializeField]
    float originalVoicePitch = 1.5f;

    private CharacterSoundEffect characterSoundEffect;
    private float health;

    // Start is called before the first frame update
    void Start()
    {
        health = maximumHealth;
    }

    public void TakeDamage(float damage,float UpForce=1f, float BackForce=10f)
    {
        ApplyDamageEffect(damage, UpForce, BackForce);
        
        if (health <= 0) {
            Die();
        }
    }

    private void Die() {
        respawnHandler.RespawnPlayer(this.gameObject);
    }

    // Turn Character to Red and move him to the back
    private void ApplyDamageEffect(float damage, float UpForce, float BackForce)
    {
        health -= damage;

        if (playerBody.TryGetComponent<Renderer>(out Renderer renderer))
        {
            renderer.material = damagedMaterial;
        }

        // Add Forces on Player when hit
        StartCoroutine(MoveBackwardAndUp(UpForce, BackForce));

        // Add Hit Noise
        characterSoundEffect = GetComponent<CharacterSoundEffect>();
        characterSoundEffect.PlayHitSound(originalVoicePitch);
    }

    private IEnumerator MoveBackwardAndUp(float UpForce, float BackForce)
    {
        // Calculate Player Direction
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = transform.position
            - transform.forward * BackForce // Vers l'arrière
            + transform.up * UpForce;      // Vers le haut

        float elapsedTime = 0f;

        while (elapsedTime < MovementDuration)
        {
            // Interpolation between  initial position and target
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / MovementDuration);

            // Increment time
            elapsedTime += Time.deltaTime;

            // Wait next frame
            yield return null;
        }

        // Verify player is on target
        transform.position = targetPosition;

        StartCoroutine(RestoreNormalStateAfterDelay());
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

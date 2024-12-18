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
    private float forceMagnitude = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has been hit by the Enemy.");
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

        //TODO force on Player to go back
        player.attachedRigidbody.AddForce(Vector3.back * forceMagnitude, ForceMode.Impulse);

        //TODO Player Health (component) -1
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

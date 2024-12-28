using UnityEngine;
using System.Collections;

public class PieceManager : MonoBehaviour
{
    [SerializeField]
    private PiecePooler piecePooler;
    [SerializeField]
    private float pieceValue=1f;
    [SerializeField]
    private AudioClip pieceSound;
    private AudioSource audioSource;
    private Renderer rend;

    void Start()
    {
        // If the AudioSource is not assigned, get it from the parent
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            rend = GetComponent<Renderer>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is the Player
        if (other.CompareTag("Player"))
        {
            // Add points to the score
            Score.piecePoints += pieceValue;

            // Log the message
            Debug.Log("Piece collected");

            // Play the sound immediately
            if (audioSource != null && pieceSound != null)
            {
                // Set a random pitch for the sound
                audioSource.pitch = Random.Range(0.95f, 1.05f);
                audioSource.clip = pieceSound;
                audioSource.PlayOneShot(pieceSound);
                rend.enabled = false;

                // Start the coroutine to wait for the sound to finish before destroying the object
                StartCoroutine(DestroyPieceAfterSound());
            }
            else
            {
                Debug.LogWarning("AudioSource or AudioClip not assigned");
            }
        }
    }

    private IEnumerator DestroyPieceAfterSound()
    {
        // Wait for the duration of the sound to finish
        yield return new WaitForSeconds(pieceSound.length);

        // Return the piece to the pool (deactivate it or destroy it)
        piecePooler.ReturnPieceToPool(this.gameObject);
    }

    public void SetPiecePooler(PiecePooler pp)
    {
        piecePooler = pp;
    }
}

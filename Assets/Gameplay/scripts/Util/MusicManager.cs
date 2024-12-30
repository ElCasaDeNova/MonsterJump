using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource;

    public static MusicManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        // Check for existing instance
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        instance = this; // Set this as the singleton instance
        DontDestroyOnLoad(gameObject); // Prevent destruction on scene load

        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        // Debugging to verify persistence
        Debug.Log("MusicManager created and set to persist: " + gameObject.name);
    }

    void Start()
    {
        // Ensure the AudioSource is playing
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.loop = true; // Loop the music
            audioSource.Play(); // Start playing the music
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject menuButtons;
    [SerializeField]
    private GameObject gameUI;
    [SerializeField]
    private GameObject settingsPanel;
    [SerializeField] 
    private Camera mainCamera;
    [SerializeField]
    private AudioClip audioClip;
    [SerializeField]
    private Chronometer chronometer;

    private AudioSource audioSource;
    private float randomVariant;

    private bool isPaused = false;

    void Start()
    {
        pauseMenu.SetActive(false);  // Hide the pause menu initially
        audioSource = GetComponent<AudioSource>();

        //Hide Cursor at the beginning of the game
        Cursor.visible = false;

        // Start Chronometer
        chronometer.StartChronometer();
    }

    void Update()
    {
        // Check if the Escape key is pressed and game not lost
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        // Make the cursor visible
        Cursor.visible = true;

        Time.timeScale = 0f;  // Stop the game time (pause the game)
        pauseMenu.SetActive(true);  // Show the pause menu
        isPaused = true;

        PlaySound();
        gameUI.SetActive(false);  // Activate the game menu UI

        chronometer.StopChronometer();
    }

    public void ResumeGame()
    {
        

        // Hide Cursor
        Cursor.visible = false;

        Time.timeScale = 1f;  // Resume the game time
        pauseMenu.SetActive(false);  // Hide the pause menu
        isPaused = false;

        PlaySound();

        // if Settings are Opened
        if (settingsPanel.activeSelf)
        {
            OpenSettings(); // Close Settings
        }

        gameUI.SetActive(true);  // Reactivate the game menu UI

        chronometer.StartChronometer();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting the game...");
        PlaySound();
        Application.Quit();
    }

    public void Restart()
    {
        Debug.Log("Reloading Game...");
        PlaySound();
        SceneLoader.nextScene = "WalkingScene";
        SceneManager.LoadScene("LoadingScene");
    }

    public void ReturnToMenu()
    {
        Debug.Log("Returning to Menu...");
        PlaySound();
        SceneLoader.nextScene = "Menu";
        SceneManager.LoadScene("LoadingScene");
    }

    public void OpenSettings()
    {
        Debug.Log("Settings Opened!");
        PlaySound();
        settingsPanel.SetActive(!settingsPanel.activeSelf);
        menuButtons.SetActive(!menuButtons.activeSelf);
    }

    private void PlaySound()
    {
        // Give variety to sound
        randomVariant = Random.Range(0.5f, 2f);
        audioSource.pitch = randomVariant;

        Debug.Log("randomVariant is " + randomVariant);
        Debug.Log("audioSource pitch is " + audioSource.pitch);

        audioSource.PlayOneShot(audioClip);

        // Set to default
        audioSource.pitch = 1;
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject settingsPanel;
    [SerializeField]
    private GameObject menuButtons;

    [SerializeField]
    private AudioClip audioClip;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayGame()
    {
        PlaySound();
        SceneLoader.nextScene = "FirstLevel";
        SceneManager.LoadScene("LoadingScene");
    }

    public void OpenSettings()
    {
        Debug.Log("Settings Opened!");
        PlaySound();
        settingsPanel.SetActive(!settingsPanel.activeSelf);
        menuButtons.SetActive(!menuButtons.activeSelf);
    }

    public void ExitGame()
    {
        Debug.Log("Game Quit!");
        PlaySound();
        Application.Quit();
    }

    private void PlaySound()
    {
        // Give variety to sound
        float randomVariant = Random.Range(0.8f, 1.2f);
        audioSource.pitch = randomVariant;
        audioSource.PlayOneShot(audioClip);

        // Set to default
        audioSource.pitch = 1;
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown qualityDropdown;
    [SerializeField]
    private Scrollbar musicVolumeScrollbar;
    [SerializeField]
    private Scrollbar generalVolumeScrollbar;
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private AudioClip audioClip;
    private AudioSource audioSource;
    [SerializeField]
    private float maxDBMusic = -10f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Get PlayerRef value or default (used one)
        int storedQualityIndex = PlayerPrefs.GetInt("QualityIndex", QualitySettings.GetQualityLevel());
        // Get PlayerRef value or default
        float storedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float storedGeneralVolume = PlayerPrefs.GetFloat("GeneralVolume", 1f);

        // Apply value to dropdown List
        qualityDropdown.value = storedQualityIndex;
        // Apply values to scrollbars
        musicVolumeScrollbar.value = storedMusicVolume;
        generalVolumeScrollbar.value = storedGeneralVolume;

        qualityDropdown.onValueChanged.AddListener(SetQuality);
        musicVolumeScrollbar.onValueChanged.AddListener(SetMusicVolume);
        generalVolumeScrollbar.onValueChanged.AddListener(SetGeneralVolume);
    }

    public void SetQuality(int qualityIndex)
    {
        PlaySound();
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualityIndex", qualityIndex);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float scrollbarValue)
    {
        float maxLinearVolume = Mathf.Pow(10, maxDBMusic / 20f);
        float linearVolume = scrollbarValue * maxLinearVolume;
        float dBVolume = (linearVolume > 0.0001f) ? Mathf.Log10(linearVolume) * 20 : -80f;
        audioMixer.SetFloat("MusicVolume", dBVolume);

        PlayerPrefs.SetFloat("MusicVolume", scrollbarValue);
        PlayerPrefs.Save();
    }

    public void SetGeneralVolume(float volume)
    {
        float dBVolume = (volume > 0.0001f) ? Mathf.Log10(volume) * 20 : -80f;
        audioMixer.SetFloat("GeneralVolume", dBVolume);

        PlayerPrefs.SetFloat("GeneralVolume", volume);
        PlayerPrefs.Save();
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

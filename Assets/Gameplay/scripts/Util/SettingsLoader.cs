using UnityEngine;
using UnityEngine.Audio;

public class SettingsLoader : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;
    private float maxDBMusic = -10f;

    // Start is called before the first frame update
    void Start()
    {
        ApplySettings();
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetMusicVolume(float scrollbarValue)
    {
        float maxLinearVolume = Mathf.Pow(10, maxDBMusic / 20f);
        float linearVolume = scrollbarValue * maxLinearVolume;
        float dBVolume = (linearVolume > 0.0001f) ? Mathf.Log10(linearVolume) * 20 : -80f;
        audioMixer.SetFloat("MusicVolume", dBVolume);
    }

    public void SetGeneralVolume(float volume)
    {
        float dBVolume = (volume > 0.0001f) ? Mathf.Log10(volume) * 20 : -80f;
        audioMixer.SetFloat("GeneralVolume", dBVolume);
    }

    public void ApplySettings()
    {
        // Get PlayerRef value or default (used one)
        int storedQualityIndex = PlayerPrefs.GetInt("QualityIndex", QualitySettings.GetQualityLevel());

        // Get PlayerRef value or default
        float storedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float storedGeneralVolume = PlayerPrefs.GetFloat("GeneralVolume", 1f);

        // Apply volumes
        SetMusicVolume(storedMusicVolume);
        SetGeneralVolume(storedGeneralVolume);
        // Apply Quality
        SetQuality(storedQualityIndex);
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField]
    private Slider slider_Music;
    [SerializeField]
    private Slider slider_sfx;
    [SerializeField]
    private TMP_Dropdown dropdown_quality;

    private void Start()
    {
        if (slider_Music != null)
            slider_Music.onValueChanged.AddListener(OnMusicVolumeChanged);
        if (slider_sfx != null)
            slider_sfx.onValueChanged.AddListener(OnSFXValueChanged);
        if (dropdown_quality != null)
            dropdown_quality.onValueChanged.AddListener(OnQualitySettingsChanged);
    }

    private void OnMusicVolumeChanged(float volume)
    {
        if (AudioManager.Singleton != null)
            AudioManager.Singleton.OnMusicVolumeChanged(volume);
    }

    private void OnSFXValueChanged(float volume)
    {
        if (AudioManager.Singleton != null)
            AudioManager.Singleton.OnSFXVolumeChanged(volume);
    }

    private void OnQualitySettingsChanged(int value)
    {
        if (value == 0)
        {
            QualitySettings.SetQualityLevel(4);
        }
        else if (value == 1)
        {
            QualitySettings.SetQualityLevel(2);
        }
        else if(value == 2)
        {
            QualitySettings.SetQualityLevel(0);
        }
    }

    public void OnClose()
    {
        if (AudioManager.Singleton != null)
            AudioManager.Singleton.HideSettings();
    }
}






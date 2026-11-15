using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Singleton;

    public AudioClip clip_background;
    public AudioClip clip_fight;
    

    // Add more audioclips here
    [SerializeField]
    private AudioSource as_background;
    [SerializeField]
    private AudioSource as_game;
    [SerializeField]
    private AudioSource as_sound;

    [SerializeField]
    private GameObject go_SettingsHolder;
    private void Awake()
    {
        if(Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBackgroundMusic()
    {
        if (as_background != null)
            as_background.Play();
    }

    public void StopBackgroundMusic()
    {
        if (as_background != null)
            as_background.Stop();
    }

    public void PlayGameMusic(AudioClip clip)
    {
        if (as_game == null)
            return;

        as_game.clip = clip;
        as_game.Play();
    }

    public void StopGameMusic()
    {
        if (as_game == null)
            return;

        as_game.clip = null;
        as_game.Stop();
    }

    public void PlayOneShotSFX(AudioClip clip)
    {
        if (as_sound != null && clip != null)
            as_sound.PlayOneShot(clip);
    }

    public void OnMusicVolumeChanged(float volume)
    {
        if (as_background != null)
            as_background.volume = volume;
        if (as_game != null)
            as_game.volume = volume;
    }

    public void OnSFXVolumeChanged(float vol)
    {
        if (as_sound != null)
            as_sound.volume = vol;
    }

    public void ShowSettings()
    {
        if (go_SettingsHolder != null)
            go_SettingsHolder.SetActive(true);
    }

    public void HideSettings()
    {
        if (go_SettingsHolder != null)
            go_SettingsHolder.SetActive(false);
    }
}

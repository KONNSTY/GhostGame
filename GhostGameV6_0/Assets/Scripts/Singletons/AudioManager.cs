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
        as_background.Play();
    }

    public void StopBackgroundMusic()
    {
        as_background.Stop();
    }

    public void PlayGameMusic(AudioClip clip)
    {
        as_game.clip = clip;

        as_game.Play();

    }

    public void StopGameMusic()
    {
        as_game.clip = null;
        as_game.Stop();
    }

    public void PlayOneShotSFX(AudioClip clip)
    {
        as_sound.PlayOneShot(clip);
    }

    public  void OnMusicVolumeChanged(float volume)
    {
        as_background.volume = volume;
        as_game.volume = volume;
    }

    public void OnSFXVolumeChanged(float vol)
    {
        as_sound.volume = vol;
    }


    public void ShowSettings()
    {
        go_SettingsHolder.SetActive(true);
    }

    public void HideSettings()
    {
        go_SettingsHolder.SetActive(false);
    }
}

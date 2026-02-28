using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    public static AudioManagerScript instance;

    [Header("----- Audio Source -----")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("----- Audio Clip -----")]
    public AudioClip background;
    public AudioClip mainMenuClick;
    public AudioClip taskOpen;
    public AudioClip taskCorrect;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }
    public void playSFX(AudioClip clip) {
        SFXSource.PlayOneShot(clip);
    }

    public void PauseMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Pause();
        }
    }

    public void UnpauseMusic()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.UnPause();
        }
    }
}

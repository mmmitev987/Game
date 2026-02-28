using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettingsScript : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSliderPauseMenu;
    [SerializeField] private Slider SFXSliderPauseMenu;

    [SerializeField] private Slider musicSliderMainMenu;
    [SerializeField] private Slider SFXSliderMainMenu;




    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume")) {
            LoadVolume();
        }
        else
        {
            SetMusicVolume(true);
            SetSFXVolume(true);
        }
    }
    public void SetMusicVolume(bool sliderFromMainMenu) 
    {
        float volume;
        if (sliderFromMainMenu) // Od main menu sliderot
        {
            volume = musicSliderMainMenu.value;
            musicSliderPauseMenu.value = volume;
            //myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
            //PlayerPrefs.SetFloat("musicVolume", volume);
        }
        else
        { // Od pause menu sliderot
            volume = musicSliderPauseMenu.value;
            musicSliderMainMenu.value = volume;
        }
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);

    }
    public void SetSFXVolume(bool sliderFromMainMenu)
    {
        float volume;
        if (sliderFromMainMenu)
        {
            volume = SFXSliderMainMenu.value;
            SFXSliderPauseMenu.value = volume;
        }
        else
        { // Od pause menu sliderot
            volume = SFXSliderPauseMenu.value;
            SFXSliderMainMenu.value = volume;

        }
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
    //public void SetSFXVolume()
    //{
    //    float volume = SFXSliderPauseMenu.value;
    //    myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    //    PlayerPrefs.SetFloat("SFXVolume", volume);
    //}


    private void LoadVolume() {
        float t1 = PlayerPrefs.GetFloat("musicVolume"), t2 = PlayerPrefs.GetFloat("SFXVolume");
        musicSliderPauseMenu.value = t1;
        SFXSliderPauseMenu.value = t2;

        musicSliderMainMenu.value = t1;
        SFXSliderMainMenu.value = t2;

        SetMusicVolume(true);
        SetSFXVolume(true);
    }

}

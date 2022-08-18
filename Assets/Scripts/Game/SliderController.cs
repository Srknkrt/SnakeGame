using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] AudioSource eatSource;
    [SerializeField] AudioSource chestSource;
    [SerializeField] AudioSource clickSource;

    [SerializeField] AudioSource musicSource;

    [SerializeField] Slider soundSlider;
    [SerializeField] Slider musicSlider;

    public void SoundChangedVolume()
    {
        eatSource.volume = soundSlider.value * 10;
        clickSource.volume = soundSlider.value * 3;
        chestSource.volume = soundSlider.value * 3;

        PlayerPrefs.SetFloat("SoundVolume", soundSlider.value);
    }

    public void MusicChangedVolume()
    {
        musicSource.volume = musicSlider.value;

        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }
}

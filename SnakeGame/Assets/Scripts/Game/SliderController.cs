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
        eatSource.volume = soundSlider.value;
        clickSource.volume = soundSlider.value / 5;
        chestSource.volume = soundSlider.value;

        PlayerPrefs.SetFloat("SoundVolume", soundSlider.value);
    }

    public void MusicChangedVolume()
    {
        musicSource.volume = musicSlider.value;

        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }
}

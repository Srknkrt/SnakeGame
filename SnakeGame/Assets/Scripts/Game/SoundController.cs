using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField] AudioSource eatSource;
    [SerializeField] AudioSource chestSource;
    [SerializeField] AudioSource clickSource;

    [SerializeField] AudioSource musicSource;

    [SerializeField] List<AudioClip> chestSounds;

    [SerializeField] Slider soundSlider;
    [SerializeField] Slider musicSlider;

    private void Start()
    {
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume");
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
    }

    public void EatSoundPlay()
    {
        eatSource.Play();
    }

    public void ChestSoundPlay()
    {
        chestSource.clip = chestSounds[Random.Range(0, chestSounds.Count)];
        chestSource.Play();
    }

    public void ClickSoundPlay()
    {
        clickSource.Play();
    }



    public void MusicStop()
    {
        musicSource.Stop();
    }
}

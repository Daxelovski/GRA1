using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Slider slider;

    void Start()
    {
        GameSave.ApplyAudioSettings();

        if(slider != null)
        {
            slider.value = GameSave.AudioVolume;
        }

        if(audioSource != null)
        {
            audioSource.volume = GameSave.AudioVolume;
        }
    }

    public void ChangeAudio()
    {
        if(slider == null)
        {
            return;
        }

        GameSave.AudioVolume = slider.value;

        if(audioSource != null)
        {
            audioSource.volume = slider.value;
        }

        PlayerPrefs.Save();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioClip playerDamageClip;
    [SerializeField] private AudioClip playerDeathClip;

    public void PlayAudio(int index, float volume)
    {
        if(!GameSave.AudioEnabled || audioSource == null || audioClips == null)
        {
            return;
        }

        if(index < 0 || index >= audioClips.Length || audioClips[index] == null)
        {
            return;
        }

        audioSource.PlayOneShot(audioClips[index], volume * GameSave.AudioVolume);
    }

    public float PlayPlayerDamage(float volume)
    {
        return PlayClip(playerDamageClip, volume);
    }

    public float PlayPlayerDeath(float volume)
    {
        return PlayClip(playerDeathClip, volume);
    }

    private float PlayClip(AudioClip clip, float volume)
    {
        if(!GameSave.AudioEnabled || audioSource == null || clip == null)
        {
            return 0f;
        }

        audioSource.PlayOneShot(clip, volume * GameSave.AudioVolume);
        return clip.length;
    }
}

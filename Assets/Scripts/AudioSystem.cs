using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;

    public void PlayAudio(int index, float volume)
    {
        audioSource.PlayOneShot(audioClips[index], volume);
    }
}

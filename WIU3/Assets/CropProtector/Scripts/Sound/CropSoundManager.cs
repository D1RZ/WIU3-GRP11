using System.Collections.Generic;
using UnityEngine;

public class CropSoundManager : MonoBehaviour
{
    public static CropSoundManager instance;

    [SerializeField] private List<AudioSource> soundFXObject; // creates an array of audio sources

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void PlaySoundFXClip(AudioClip soundEffect, Transform soundSourceLocation,float volume)
    {
        // spawn in gameObject
        for (int i = 0; i < soundFXObject.Count; i++)
        {
            if (!soundFXObject[i].isPlaying)
            {
                audioSource = Instantiate(soundFXObject[i], soundSourceLocation.position, Quaternion.identity);
                break;
            }
        }

        // assign the audioClip
        audioSource.clip = soundEffect;

        audioSource.volume = volume;
        
        // play sound
        audioSource.Play();

        // get length of sound FX clip
        float clipLength = audioSource.clip.length;

        // destroy the clip after it is done playing
        Destroy(audioSource.gameObject, clipLength);
    }
}
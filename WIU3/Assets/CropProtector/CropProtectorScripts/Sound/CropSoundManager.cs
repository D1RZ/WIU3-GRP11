using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropSoundManager : MonoBehaviour
{
    public static CropSoundManager instance;

    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if(instance == null)
        instance = this;
    }
    public void PlaySoundFXClip(AudioClip soundEffect, Transform soundSourceLocation, float volume)
    {
        // spawn in gameObject
        AudioSource audioSource = Instantiate(soundFXObject, soundSourceLocation.position, Quaternion.identity);

        // assign the audioClip
        audioSource.clip = soundEffect;

        // assign the volume
        audioSource.volume = volume;

        // play sound
        audioSource.Play();

        // get length of sound FX clip
        float clipLength = audioSource.clip.length;

        // destroy the clip after it is done playing
        Destroy(audioSource.gameObject,clipLength);
    }
}

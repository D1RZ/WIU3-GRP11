using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    private AudioSource audio;
    void Start()
    {

        StartCoroutine(LoopAudio());
    }

    IEnumerator LoopAudio()
    {
        audio = GetComponent<AudioSource>();
        float length = audio.clip.length;

        while (true)
        {
            audio.Play();
            yield return new WaitForSeconds(length);
        }
    }
}

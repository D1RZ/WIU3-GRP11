using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    private AudioSource audio;
    public bool gameFinished;
    void Start()
    {
        gameFinished = false;
        StartCoroutine(LoopAudio());
    }

    IEnumerator LoopAudio()
    {
        audio = GetComponent<AudioSource>();
        float length = audio.clip.length;

        while (gameFinished == false)
        {
            audio.Play();
            yield return new WaitForSeconds(length);
        }
    }
}

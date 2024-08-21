using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacteriaBGMVolAdjuster : MonoBehaviour
{
    [SerializeField] private List<AudioSource> audioSource;

    [SerializeField] AudioSettingsManager audioSettingsManager;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < audioSource.Count; i++)
        {
            audioSource[i].volume = audioSettingsManager.GetBGM();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMVolumeAdjuster : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] AudioSettingsManager audioSettingsManager;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = audioSettingsManager.GetBGM();
    }
}

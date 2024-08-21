using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public TextMeshProUGUI MusicText;

    public Music[] musics;

    private int CurrentMusicID;
    private void Awake()
    {
        foreach (var m in musics)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.clip = m.clip;
        }
    }
    private void Update()
    {
        if(MusicText != null)
        {
            MusicText.text = musics[CurrentMusicID].clip.ToString();
        }
    }

    public void NextMusic()
    {
        StopMusic();
        //ChangeMusicID
        CurrentMusicID++;
        if (CurrentMusicID > musics.Count())
        {
            CurrentMusicID = 0;
        }
        //PLaymusic
        PlayMusic();
    }
    public void PreviousMusic()
    {
        StopMusic();
        //ChangeMusicID
        CurrentMusicID--;
        if( CurrentMusicID < 0)
        {
            CurrentMusicID = musics.Count() - 1;
        }
        //PLaymusic
        PlayMusic();
    }
    public void PlayMusic()
    {
        foreach (var m in musics)
        {
            if (m.MusicId == CurrentMusicID)
            {
                m.source.Play();
                break;
            }
        }
    }
    public void StopMusic()
    {
        foreach (var m in musics)
        {
            if (m.MusicId == CurrentMusicID)
            {
                m.source.Stop();
                break;
            }
        }
    }
    public void RandomisedMusic()
    {
        int randomInt = Random.Range(0, musics.Count() - 1);
        foreach (var m in musics)
        {
            if (m.MusicId == randomInt)
            {
                m.source.Play();
                break;
            }
        }
    }
    public bool CheckCurrentMusicHasStopped()
    {
        return musics[CurrentMusicID].source.isPlaying;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallMusicFunc : MonoBehaviour
{
    [SerializeField] private MusicManager InGameBgm;
    private bool FirstTime = true;
    // Update is called once per frame
    void Update()
    {
        if(FirstTime)
        {
            InGameBgm.RandomisedMusic();
            FirstTime = false;
        }
        else
        {
            if (InGameBgm.CheckCurrentMusicHasStopped())
                InGameBgm.RandomisedMusic();
        }
    }
}

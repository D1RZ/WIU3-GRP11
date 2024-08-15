using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour // CODE IS BROKEN -----------------------------------------------------
{
    public GameObject loadingScreen;

    //private void Start()
    //{
    //    // Initially hide the loading screen
    //    if (loadingScreen != null)
    //    {
    //        loadingScreen.SetActive(false);
    //    }
    //}

    public void ShowLoadingScreen()
    {
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }
    }

    public void HideLoadingScreen()
    {
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }
}

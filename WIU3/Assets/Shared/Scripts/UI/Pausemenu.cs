using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pausemenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
                Resume();
            else
                Pause();
        }
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void Options()
    {
        Debug.Log("Loading Options....");
    }
    public void Controls()
    {
        Debug.Log("Loading Controls....");
    }
    public void Credits()
    {
        Debug.Log("Loading Credits....");
    }
    public void QuitGame()
    {
        Debug.Log("Quiting Game....");
    }
}

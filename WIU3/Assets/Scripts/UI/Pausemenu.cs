using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pausemenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject OptionmenuUI;
    public GameObject ControlmenuUI;
    public GameObject CreditmenuUI;
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
        Debug.Log("Activated Resume");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    private void Pause()
    {
        Debug.Log("Activated Pause");
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void Options()
    {
        OptionmenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        Debug.Log("Loading Options....");
    }
    public void Controls()
    {
        ControlmenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        Debug.Log("Loading Controls....");
    }
    public void Credits()
    {
        CreditmenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        Debug.Log("Loading Credits....");
    }
    public void QuitGame()
    {
        pauseMenuUI.SetActive(false);
        Debug.Log("Quiting Game....");
    }
    public void back(GameObject CurrentGameMenuUI)
    {
        Pause();
        CurrentGameMenuUI.SetActive(false);
    }
}

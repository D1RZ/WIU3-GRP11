using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public static bool OtherMenuopened = false;
    private GameObject CurrentGameMenu;
    public GameObject OptionmenuUI;
    public GameObject ControlmenuUI;
    public GameObject CreditmenuUI;
    public GameObject pauseMenuUI;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
                Resume();
            if (OtherMenuopened)
                back(CurrentGameMenu);
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
        CurrentGameMenu = OptionmenuUI;
        OtherMenuopened = true;
        OptionmenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        Debug.Log("Loading Options....");
    }
    public void Controls()
    {
        CurrentGameMenu = ControlmenuUI;
        OtherMenuopened = true;
        ControlmenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        Debug.Log("Loading Controls....");
    }
    public void Credits()
    {
        CurrentGameMenu = CreditmenuUI;
        OtherMenuopened = true;
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

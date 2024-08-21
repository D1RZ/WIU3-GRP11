using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject OptionsUI;
    public static bool OptionsMenuopened;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(OptionsMenuopened)
                BackMainMenu();
            else
                QUITGame();
        }
    }
    public void Options()
    {
        OptionsUI.SetActive(true);
        OptionsMenuopened = true;
    }
    public void PLayGame()
    {
        mainMenuUI.SetActive(false);
        SceneManager.LoadScene("Arcade", LoadSceneMode.Single);
    }
    public void QUITGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
    public void BackMainMenu()
    {
        OptionsUI.SetActive(false);
        OptionsMenuopened = false;
    }
}

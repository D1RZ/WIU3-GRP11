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
        mainMenuUI.SetActive(false);
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
        Application.Quit();
    }
    public void BackMainMenu()
    {
        mainMenuUI.SetActive(true);
        OptionsUI.SetActive(false);
        OptionsMenuopened = false;
    }
}

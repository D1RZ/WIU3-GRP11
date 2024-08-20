using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public static bool OtherMenuopened = false;
    private GameObject CurrentGameMenu;
    public GameObject OptionmenuUI;
    public GameObject ControlmenuUI;
    public GameObject CreditmenuUI;
    public GameObject pauseMenuUI;
    public GameObject DialogueUI;
    public TextMeshProUGUI DialogueTexter;
    
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
        Resume();
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
        Debug.Log("Quiting Game....");
    }
    public void back(GameObject CurrentGameMenuUI)
    {
        Pause();
        CurrentGameMenuUI.SetActive(false);
    }
    public void TextPlay(string DialogueText)
    {
        DialogueUI.SetActive(true);
        StartCoroutine(TypeText(DialogueText));
        //Add voices
    }
    private IEnumerator TypeText(string fullText)
    {
        string currentLine = ""; // Current line being formed
        int charCount = 0; // Count of characters in the current line

        // Loop through each character in the full text
        foreach (char letter in fullText)
        {
            // Check if adding the next character would exceed the character limit
            if (charCount >= 86 && letter != ' ') // Avoid breaking words
            {
                DialogueTexter.text = currentLine + "\n" + "~~Press SPACE To Continue~~";
                // Wait for user input to continue to the next line
                yield return WaitForInput();

                // Clear the current line for the next sentence
                currentLine = "";
                charCount = 0; // Reset character count after clearing
            }

            // Add the letter to the current line
            currentLine += letter;
            charCount++; // Update character count

            // Update the TextMeshPro text to show typed characters
            DialogueTexter.text = currentLine; // Append prompt message;
            yield return new WaitForSeconds(0.1f); // Wait for a specified duration
        }
        DialogueTexter.text = currentLine + "\n" + "~~Press SPACE To Continue~~";
        // Final wait for input before finishing
        yield return WaitForInput();
        DialogueTexter.text = ""; // Clear text after final input
        DialogueUI.SetActive(false);
    }
    private IEnumerator WaitForInput()
    {
        // Wait for user input to clear text and continue
        while (!Input.GetKeyDown(KeyCode.Space)) // Change KeyCode.Space to any key you prefer
        {
            yield return null; // Wait until the input is detected
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JukeBoxTrigger : MonoBehaviour
{
    public GameObject JukeboxMenu;
    private bool isInteract = false;
    private bool isEnter = false;

    private void Start()
    {
    }

    private void Update()
    {
        isInteract = Input.GetKeyDown(KeyCode.E);

        Debug.Log(JukeboxMenu.activeSelf);

        if (isEnter && isInteract)
        {
            // Call Loading Screen here
            StartCoroutine(DelaySeconds(3f));
            // Close loading screen here
            // Task 2c - Load the scene using SceneManager.LoadScene()
            JukeboxMenu.SetActive(true);
        }
        else if (!isEnter)
        {
            JukeboxMenu.SetActive(false);
        }
        else if (isInteract && isEnter && JukeboxMenu.activeSelf == true)
        {
            JukeboxMenu.SetActive(false);
        }

    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        isEnter = true;
        Debug.Log("Enter OnTriggerEnter2D");

    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        isEnter = false;
        Debug.Log("Exit OnTrigger");
    }

    private IEnumerator DelaySeconds(float delayAmt)
    {
        yield return new WaitForSeconds(delayAmt);
    }
}

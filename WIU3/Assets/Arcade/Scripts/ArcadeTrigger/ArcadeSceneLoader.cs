using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArcadeSceneLoader : MonoBehaviour
{
    [SerializeField] public GameObject playPrompt;
    private RectTransform rt;
    [SerializeField] public Vector2 promptPos;
    [SerializeField] string SceneName;
    private bool isInteract = false;
    private bool isEnter = false;

    private void Start()
    {
        rt = playPrompt.GetComponent<RectTransform>();
    }

    private void Update()
    {
        isInteract = Input.GetKeyDown(KeyCode.E);

        if (isEnter && isInteract)
        {
            // Call Loading Screen here
            StartCoroutine(DelaySeconds(3f));
            // Close loading screen here
            // Task 2c - Load the scene using SceneManager.LoadScene()
            SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        isEnter = true;
        rt.anchoredPosition = promptPos;
        playPrompt.SetActive(true);
        //Debug.Log("Enter OnTriggerEnter2D");
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        isEnter = false;
        playPrompt.SetActive(false);
        //Debug.Log("Exit OnTrigger");
    }

    private IEnumerator DelaySeconds(float delayAmt)
    {
        yield return new WaitForSeconds(delayAmt);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public bool GameStart = false;
    private bool SpaceClicked = false;
    public GameObject GameStartUI;
    [SerializeField] private AudioClip StartGameClip;
    [SerializeField] AudioSettingsManager audioSettingsManager;
    [SerializeField] CropSoundManager cropSoundManager;
    [SerializeField] GameObject AudioSettingsPanel;
    private void Start()
    {
        GameStartUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!AudioSettingsPanel.activeInHierarchy)
            {
                AudioSettingsPanel.SetActive(true);
            }
            else
            {
                AudioSettingsPanel.SetActive(false);
            }
        }
        SpaceClicked = Input.GetKeyDown(KeyCode.Space);
        if(SpaceClicked )
            StartGame();
    }
    private void StartGame()
    {
        GameStart = true;
        GameStartUI.SetActive(false);
        cropSoundManager.PlaySoundFXClip(StartGameClip, transform, audioSettingsManager.GetSFX());
    }
}

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
    private void Start()
    {
        GameStartUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        SpaceClicked = Input.GetKeyDown(KeyCode.Space);
        if(SpaceClicked )
            StartGame();
    }
    private void StartGame()
    {
        GameStart = true;
        GameStartUI.SetActive(false);
        //cropSoundManager.PlaySoundFXClip(StartGameClip, transform, audioSettingsManager.GetSFX());
    }
}

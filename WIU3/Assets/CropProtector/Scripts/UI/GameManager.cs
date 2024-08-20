using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerData playerData;

    [SerializeField] Image PlayerHealthBar;

    [SerializeField] Image CropHealthBar;

    [SerializeField] Crop Crop;

    [SerializeField] PlayerController playerController;

    [SerializeField] MovementController movementController;

    [SerializeField] Animator playerAnimator;

    [SerializeField] List<GameObject> enemySpawners;

    [SerializeField] TextMeshProUGUI currentWave;

    [SerializeField] TextMeshProUGUI bannerText;

    [SerializeField] TextMeshProUGUI LocustCountText;

    [SerializeField] GameObject MakeScreenDarkerPanel;

    [SerializeField] GameObject EndGameUI;

    [SerializeField] AudioClip GameOverSound;

    [SerializeField] GameObject BGM;

    [SerializeField] GameObject AudioSettingsPanel;

    private AudioSettingsManager audioSettingsManager;

    public int LocustSpawnCount = 0;

    public int LocustMaxCount = 10;

    public int LocustCount = 10;

    private float CurrentWave = 1;

    bool GameOver = false;

    private void Start()
    {
        currentWave.text = CurrentWave.ToString();

        bannerText.text = "";

        LocustCountText.text = LocustCount.ToString();

        audioSettingsManager = AudioSettingsPanel.gameObject.GetComponent<AudioSettingsManager>();

        audioSettingsManager.Load();

        AudioSettingsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        audioSettingsManager.Awake();

        if (Crop.CropCurrentHealth <= 0 || playerData.health <= 0)
        {
            if (!GameOver)
            {
                BGM.SetActive(false);

                CropSoundManager.instance.PlaySoundFXClip(GameOverSound, transform,audioSettingsManager.GetSFX());

                GameOver = true;
            }

            if (playerData.health <= 0)
            PlayerHealthBar.rectTransform.sizeDelta = new Vector2(0, 38);

            if (playerController != null)
            {
                playerController.enabled = false;

                if (movementController != null)
                    movementController._PlayerRB.constraints = RigidbodyConstraints2D.FreezeAll;

                if(playerData.health >= 0 && Crop.CropCurrentHealth <= 0)
                playerAnimator.enabled = false;
            }

            if (enemySpawners != null)
            {
                for (int i = 0; i < enemySpawners.Count; i++)
                {
                    enemySpawners[i].SetActive(false);
                }
            }

            if (playerData.health >= 0 && Crop.CropCurrentHealth <= 0)
            {

                bannerText.text = "Better luck next time!";

                MakeScreenDarkerPanel.SetActive(true);

                EndGameUI.SetActive(true);
            }
            else
            {
                playerAnimator.SetBool("death", true);
                playerAnimator.SetBool("idle", false);
                playerAnimator.SetBool("move", false);
            }

            return;
        }

        LocustCountText.text = LocustCount.ToString();

        if (LocustCount == 0 && CurrentWave != 2)
        {
            CurrentWave += 1;
            currentWave.text = CurrentWave.ToString();
            LocustCount = 10;
            LocustSpawnCount = 0;
        }
        else if(LocustCount == 0 && CurrentWave == 2)
        {
            if (playerController != null)
            {
                playerController.enabled = false;

                if (movementController != null)
                    movementController._PlayerRB.constraints = RigidbodyConstraints2D.FreezeAll;

                playerAnimator.enabled = false;
            }

            if (enemySpawners != null)
            {
               for (int i = 0; i < enemySpawners.Count; i++)
               {
                    enemySpawners[i].SetActive(false);
               }
            }

            bannerText.text = "Well Done!";
            
            MakeScreenDarkerPanel.SetActive(true);
            
            EndGameUI.SetActive(true);
        }

        PlayerHealthBar.rectTransform.sizeDelta = new Vector2(1.8f * playerData.health, 38);

        float CropHealthPercentage = Crop.CropCurrentHealth / Crop.CropMaxHealth * 100;

        CropHealthBar.rectTransform.sizeDelta = new Vector2(4.8f * CropHealthPercentage,38);
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

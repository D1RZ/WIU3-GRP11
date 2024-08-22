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

    [SerializeField] TextMeshProUGUI MosquitoCountText;

    [SerializeField] GameObject MakeScreenDarkerPanel;

    [SerializeField] GameObject EndGameUI;

    [SerializeField] AudioClip GameOverSound;

    [SerializeField] AudioClip WinGameSound;

    [SerializeField] GameObject InGameBGM;

    [SerializeField] GameObject VictoryBGM;

    [SerializeField] GameObject AudioSettingsPanel;

    [SerializeField] CropSoundManager cropSoundManager;

    [SerializeField] GameObject Star1;

    [SerializeField] GameObject Star2;

    [SerializeField] GameObject Star3;

    [SerializeField] private int LocusWave2MaxCount;

    [SerializeField] private int MosquitoWave2MaxCount;

    [SerializeField] private int MaxNoOfWaves;

    private AudioSettingsManager audioSettingsManager;

    public int LocustSpawnCount = 0;

    public int LocustMaxCount = 5;

    public int LocustCount = 5;

    public int MosquitoSpawnCount = 0;

    public int MosquitoMaxCount = 0;

    public int MosquitoCount = 0;

    private float CurrentWave = 1;

    bool GameOver = false;

    bool Star1AnimFinished = false;

    bool Star2AnimFinished = false;

    bool Star3AnimFinished = false;

    float animTimeElapsed = 0;

    float AnimCooldownTimer = 0.5f;

    private void Start()
    {
        currentWave.text = CurrentWave.ToString();

        bannerText.text = "";

        LocustCountText.text = LocustCount.ToString();

        audioSettingsManager = AudioSettingsPanel.gameObject.GetComponent<AudioSettingsManager>();

        audioSettingsManager.Load();

        AudioSettingsPanel.SetActive(false);

        VictoryBGM.SetActive(false);

        MakeScreenDarkerPanel.SetActive(false);

        EndGameUI.SetActive(false);

        Star1.GetComponent<RectTransform>().sizeDelta = new Vector2(0,0);

        Star2.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);

        Star3.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);

         for (int i = 0; i < enemySpawners.Count; i++)
         {
             enemySpawners[i].SetActive(false);
         }
         
         for (int i = 0; i < 3; i++)
         {
             enemySpawners[i].GetComponent<EnemySpawner>().currentSpawnType = EnemySpawner.EnemySpawnType.Locust;
             enemySpawners[i].SetActive(true);
         }
    }

    // Update is called once per frame
    void Update()
    {
        audioSettingsManager.Awake();

        if (Crop.CropCurrentHealth <= 0 || playerData.health <= 0)
        {
            if (!GameOver)
            {
                InGameBGM.SetActive(false);

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

        MosquitoCountText.text = MosquitoCount.ToString();

        if (LocustCount == 0 && MosquitoCount == 0 && CurrentWave != MaxNoOfWaves)
        {
            CurrentWave += 1;
            currentWave.text = CurrentWave.ToString();
            if(CurrentWave == 2)
            {
                LocustMaxCount = LocusWave2MaxCount;
                MosquitoMaxCount = MosquitoWave2MaxCount;
                for (int i = 3; i < 5; i++)
                {
                    enemySpawners[i].GetComponent<EnemySpawner>().currentSpawnType = EnemySpawner.EnemySpawnType.Locust;
                    enemySpawners[i].SetActive(true);
                }
                enemySpawners[1].GetComponent<EnemySpawner>().currentSpawnType = EnemySpawner.EnemySpawnType.Mosquito;
            }
            LocustCount = LocustMaxCount;
            MosquitoCount = MosquitoMaxCount;
            LocustSpawnCount = 0;
            MosquitoSpawnCount = 0;
        }
        else if(LocustCount == 0 && MosquitoCount == 0 && CurrentWave == MaxNoOfWaves)
        {
            InGameBGM.SetActive(false);

            VictoryBGM.SetActive(true);

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

            if (AnimCooldownTimer > 0)
            AnimCooldownTimer -= Time.deltaTime;

            if (!Star1AnimFinished && AnimCooldownTimer <= 0)
            {
                Star1.SetActive(true);

                animTimeElapsed += Time.deltaTime;

                if(animTimeElapsed < 1)
                {
                    Star1.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(new Vector2(0,0),new Vector2(100,100),animTimeElapsed/1);
                }
                else
                {
                    Star1AnimFinished = true;
                    animTimeElapsed = 0;
                    AnimCooldownTimer = 1;
                }
            }
            else if(Star1AnimFinished && !Star2AnimFinished && AnimCooldownTimer <= 0 && Crop.CropCurrentHealth >= 50)
            {
                Star2.SetActive(true);

                animTimeElapsed += Time.deltaTime;

                if (animTimeElapsed < 1)
                {
                    Star2.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(new Vector2(0, 0), new Vector2(100, 100), animTimeElapsed / 1);
                }
                else
                {
                    Star2AnimFinished = true;
                    animTimeElapsed = 0;
                    AnimCooldownTimer = 1;
                }
            }
            else if (Star2AnimFinished && !Star3AnimFinished && AnimCooldownTimer <= 0 && Crop.CropCurrentHealth >= 90)
            {
                Star3.SetActive(true);

                animTimeElapsed += Time.deltaTime;

                if (animTimeElapsed < 1)
                {
                    Star3.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(new Vector2(0, 0), new Vector2(100, 100), animTimeElapsed / 1);
                }
                else
                {
                    Star3AnimFinished = true;
                    animTimeElapsed = 0;
                }
            }
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

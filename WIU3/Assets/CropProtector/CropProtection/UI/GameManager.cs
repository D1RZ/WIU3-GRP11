using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; 

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerData playerData;

    [SerializeField] Image PlayerHealthBar;

    [SerializeField] Image CropHealthBar;

    [SerializeField] Crop Crop;

    [SerializeField] PlayerController playerController;

    [SerializeField] MovementController movementController;

    [SerializeField] List<GameObject> enemySpawners;

    [SerializeField] TextMeshProUGUI timer;

    private float _timeElapsed = 0;

    private float _timeRemaining = 50;

    private void Start()
    {
        timer.text = _timeRemaining.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(Crop.CropCurrentHealth <= 0)
        {
            if (playerController != null)
            {
                playerController.enabled = false;

                if (movementController != null)
                    movementController._PlayerRB.constraints = RigidbodyConstraints2D.FreezeAll;
            }

            if (enemySpawners != null)
            {
                for (int i = 0; i < enemySpawners.Count; i++)
                {
                    enemySpawners[i].SetActive(false);
                }
            }

            return;
        }

        _timeElapsed += Time.deltaTime;

        if(_timeElapsed >= 1)
        {
            _timeRemaining -= 1;
            timer.text = _timeRemaining.ToString();
            _timeElapsed = 0;
        }

        PlayerHealthBar.rectTransform.sizeDelta = new Vector2(1.8f * playerData.health, 38);

        float CropHealthPercentage = Crop.CropCurrentHealth / Crop.CropMaxHealth * 100;

        CropHealthBar.rectTransform.sizeDelta = new Vector2(4.8f * CropHealthPercentage,38);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    [SerializeField] List<EnemySpawner> enemySpawners;

    // Update is called once per frame
    void Update()
    {
        if(Crop.CropCurrentHealth <= 0)
        {
            if(playerController != null)
            playerController.enabled = false;

            if (enemySpawners != null)
            {
                for (int i = 0; i < enemySpawners.Count; i++)
                {
                    if (enemySpawners[i] != null)
                        enemySpawners[i].enabled = false;
                }
            }

            return;
        }

        PlayerHealthBar.rectTransform.sizeDelta = new Vector2(1.8f * playerData.health, 38);

        float CropHealthPercentage = Crop.CropCurrentHealth / Crop.CropMaxHealth * 100;

        CropHealthBar.rectTransform.sizeDelta = new Vector2(4.8f * CropHealthPercentage,38);
    }
}

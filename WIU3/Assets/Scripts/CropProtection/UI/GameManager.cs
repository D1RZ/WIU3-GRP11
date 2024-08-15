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

    [SerializeField] MovementController movementController;

    [SerializeField] List<GameObject> enemySpawners;

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

        PlayerHealthBar.rectTransform.sizeDelta = new Vector2(1.8f * playerData.health, 38);

        float CropHealthPercentage = Crop.CropCurrentHealth / Crop.CropMaxHealth * 100;

        CropHealthBar.rectTransform.sizeDelta = new Vector2(4.8f * CropHealthPercentage,38);
    }
}

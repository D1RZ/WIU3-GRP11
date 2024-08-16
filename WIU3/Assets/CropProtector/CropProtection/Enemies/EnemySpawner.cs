using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject Locust;

    [SerializeField] private float EnemySpawnRate;

    private GameManager gameManager;

    private float SpawnTimer;

    private void Start()
    {
        gameManager = GameObject.Find("CropGameManager").GetComponent<GameManager>();
        SpawnTimer = EnemySpawnRate;
        Instantiate(Locust, transform.position, Quaternion.identity);
        gameManager.LocustSpawnCount++;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.LocustSpawnCount != gameManager.LocustMaxCount)
        {
            SpawnTimer -= Time.deltaTime;

            if (SpawnTimer <= 0)
            {
                Instantiate(Locust, transform.position, Quaternion.identity);
                gameManager.LocustSpawnCount++;
                SpawnTimer = EnemySpawnRate;
            }
        }
    }
}

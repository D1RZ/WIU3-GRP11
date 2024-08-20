using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject Locust;

    [SerializeField] private float EnemySpawnRate;

    [SerializeField] private GameManager gameManager;

    private float SpawnTimer;

    private void Start()
    {
        SpawnTimer = EnemySpawnRate;
        Instantiate(Locust, transform.position, Quaternion.identity);
        gameManager.LocustSpawnCount += 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.LocustSpawnCount != gameManager.LocustMaxCount)
        {
            SpawnTimer -= Time.deltaTime;

            if (SpawnTimer <= 0 && gameManager.LocustSpawnCount != gameManager.LocustMaxCount)
            {
                Instantiate(Locust, transform.position, Quaternion.identity);
                gameManager.LocustSpawnCount += 1;
                SpawnTimer = EnemySpawnRate;
            }
        }
    }
}

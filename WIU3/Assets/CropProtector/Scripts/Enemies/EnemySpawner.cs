using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject Locust;

    [SerializeField] private GameObject Mosquito;

    [SerializeField] private float EnemySpawnRate;

    [SerializeField] private GameManager gameManager;

    private float SpawnTimer;

    public enum EnemySpawnType
    {
        Locust,
        Mosquito
    }

    public EnemySpawnType currentSpawnType;

    public void Start()
    {
        SpawnTimer = EnemySpawnRate;
        if (currentSpawnType == EnemySpawnType.Locust)
        {
            Instantiate(Locust, transform.position, Quaternion.identity);
            gameManager.LocustSpawnCount += 1;
        }
        else if(currentSpawnType == EnemySpawnType.Mosquito)
        {
            Instantiate(Mosquito, transform.position, Quaternion.identity);
            gameManager.MosquitoSpawnCount += 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSpawnType == EnemySpawnType.Locust)
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
        else if (currentSpawnType == EnemySpawnType.Mosquito)
        {
            if (gameManager.MosquitoSpawnCount != gameManager.MosquitoMaxCount)
            {
                SpawnTimer -= Time.deltaTime;

                if (SpawnTimer <= 0 && gameManager.MosquitoSpawnCount != gameManager.MosquitoMaxCount)
                {
                    Instantiate(Mosquito, transform.position, Quaternion.identity);
                    gameManager.MosquitoSpawnCount += 1;
                    SpawnTimer = EnemySpawnRate;
                }
            }
        }
    }
}

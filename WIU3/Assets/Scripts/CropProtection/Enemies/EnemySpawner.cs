using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject Locust;

    [SerializeField] private float EnemySpawnRate;

    private float SpawnTimer;

    private void Start()
    {
        SpawnTimer = EnemySpawnRate;
        Instantiate(Locust, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        SpawnTimer -= Time.deltaTime;

        if(SpawnTimer <= 0)
        {
            Instantiate(Locust, transform.position, Quaternion.identity);
            SpawnTimer = EnemySpawnRate;
        }
    }
}

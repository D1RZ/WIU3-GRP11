using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFloatingPlant : MonoBehaviour
{
    [SerializeField] private GameObject floatingPlant; // The prefab to spawn
    [SerializeField] private Transform spawnArea; // The transform defining the spawn area
    [SerializeField] private float minSpawnInterval = 6f; // Minimum time between spawns
    [SerializeField] private float maxSpawnInterval = 12f; // Maximum time between spawns

    private void Start()
    {
        if (floatingPlant == null || spawnArea == null)
        {
            Debug.LogError("floatingPlant or SpawnArea is not assigned!");
            return;
        }

        StartCoroutine(SpawnObjects());
    }

    private System.Collections.IEnumerator SpawnObjects()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);
            Spawn();
        }
    }

    private void Spawn()
    {
        // Get the spawn area bounds (assuming spawnArea is a rectangular area)
        Vector3 spawnPosition = new Vector3(
            Random.Range(spawnArea.position.x - spawnArea.localScale.x / 2, spawnArea.position.x + spawnArea.localScale.x / 2),
            Random.Range(spawnArea.position.y - spawnArea.localScale.y / 2, spawnArea.position.y + spawnArea.localScale.y / 2),
            spawnArea.position.z // Assuming we spawn in 2D, so z is constant
        );

        Instantiate(floatingPlant, spawnPosition, Quaternion.identity);
    }
}

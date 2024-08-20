using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFloatingPlant : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private GameObject floatingPlant;
    [SerializeField] private GameObject spawnArea;
    [SerializeField] private float minSpawnInterval = 6f;
    [SerializeField] private float maxSpawnInterval = 12f;

    private void Start()
    {
        if (floatingPlant == null || spawnArea == null)
        {
            Debug.LogError("floatingPlant or SpawnArea is not assigned!");
            return;
        }

        if (gameController.GetGameStatus() == "End")
            return;

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
        // Access the Transform component of the spawnArea GameObject
        Transform spawnTransform = spawnArea.transform;

        // Get the spawn area bounds (assuming spawnArea is a rectangular area)
        Vector3 spawnPosition = new Vector3(
            Random.Range(spawnTransform.position.x - spawnTransform.localScale.x / 2, spawnTransform.position.x + spawnTransform.localScale.x / 2),
            Random.Range(spawnTransform.position.y - spawnTransform.localScale.y / 2, spawnTransform.position.y + spawnTransform.localScale.y / 2),
            spawnTransform.position.z // Assuming we spawn in 2D, so z is constant
        );

        Instantiate(floatingPlant, spawnPosition, Quaternion.identity);
    }
}
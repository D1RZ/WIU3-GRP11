using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waste : MonoBehaviour
{
    [SerializeField] private LayerMask seaweedLayer; // Layer for detecting seaweed
    [SerializeField] private GameObject seaweedPrefab; // Seaweed prefab to spawn
    //[SerializeField] private GameObject spawnArea; // Spawn area object

    private bool hasReachedBottom = false;
    private int seaweedSpawned = 0; // Counter for tracking spawned seaweed
    private int maxSeaweedSpawn = 3; // Maximum seaweed that can be spawned
    private float fixedYPosition = -4.05f; // Fixed y-position for spawning

    private void Update()
    {
        if (!hasReachedBottom)
        {
            // Move waste downwards
            transform.Translate(Vector3.down * 1f * Time.deltaTime);

            // Check if waste reaches the bottom (y <= -5 as an example, adjust as needed)
            if (transform.position.y <= -4.4f) // Adjust this value based on your floor level
            {
                hasReachedBottom = true;
                CheckForSeaweed();
            }
        }
    }

    private void CheckForSeaweed()
    {
        // Check for nearby seaweed within a 1f radius
        Collider2D[] seaweedInRange = Physics2D.OverlapCircleAll(transform.position, 1f, seaweedLayer);
        foreach (Collider2D seaweed in seaweedInRange)
        {
            if (seaweed.CompareTag("Seaweed"))
            {
                // Spawn new seaweed nearby at the fixed y-position, within 0.1 to 1f radius from the waste object
                SpawnSeaweed();
                if (seaweedSpawned >= maxSeaweedSpawn)
                    break;
            }
        }

        // After spawning, destroy the waste object
        Destroy(gameObject);
    }

    private void SpawnSeaweed()
    {
        for (int i = 0; i < 1; i++) // Number of seaweed to spawn per detected seaweed
        {
            if (seaweedSpawned >= maxSeaweedSpawn)
                break;

            // Get a random x-offset within 0.1 to 1f distance from the waste object
            float xOffset = Random.Range(0.1f, 1f) * (Random.value > 0.5f ? 1 : -1); // Randomly move left or right
            float spawnX = transform.position.x + xOffset;

            //// Ensure the x-position is within the bounds of the spawn area
            //Transform spawnTransform = spawnArea.transform;
            //spawnX = Mathf.Clamp(
            //    spawnX,
            //    spawnTransform.position.x - spawnTransform.localScale.x / 2,
            //    spawnTransform.position.x + spawnTransform.localScale.x / 2
            //);

            Vector3 spawnPosition = new Vector3(spawnX, fixedYPosition, 0);

            Instantiate(seaweedPrefab, spawnPosition, Quaternion.identity);
            seaweedSpawned++;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the detection radius in the editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
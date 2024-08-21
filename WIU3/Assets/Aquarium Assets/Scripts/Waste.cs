using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waste : MonoBehaviour
{
    [SerializeField] private LayerMask seaweedLayer; // Layer for detecting seaweed
    [SerializeField] private GameObject seaweedPrefab; // Seaweed prefab to spawn

    private bool hasReachedBottom = false;
    private int seaweedSpawned = 0; // Counter for tracking spawned seaweed
    private int maxSeaweedSpawn = 3; // Maximum seaweed that can be spawned
    private float fixedYPosition = -4.05f; // Fixed y-position for spawning

    [SerializeField] GameController gameController;

    private void Update()
    {
        if (!hasReachedBottom)
        {
            // Move waste downwards
            transform.Translate(Vector3.down * 1f * Time.deltaTime);

            // Check if waste reaches the bottom
            if (transform.position.y <= -4.4f)
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
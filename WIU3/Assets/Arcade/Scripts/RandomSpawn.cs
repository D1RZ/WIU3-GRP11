using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public GameObject prefabToSpawn; // The prefab to spawn
    public float spawnInterval = 2f; // Time interval between spawns
    public float DeathInterval = 2f;
    private BoxCollider2D area; // The area within which to spawn the prefab
    private GameObject Tobedeleted;
    public int KillCount, currentSpawnIndex, totalAllowedSpawns;
    public TextMeshProUGUI Percentage;
    public GameObject crosshair;
    public GameObject EndgameUi;
    [SerializeField] private CountDown CountDown;

    void Start()
    {
        // Get the BoxCollider attached to the GameObject this script is attached to
        area = GetComponent<BoxCollider2D>();
        totalAllowedSpawns = Mathf.FloorToInt(CountDown.duration / spawnInterval);
        // Start the spawning process
        InvokeRepeating("SpawnPrefab", 0f, spawnInterval);
        InvokeRepeating("TimesUP", 5f, DeathInterval);
    }
    private void Update()
    {
        if(CountDown.timeRemaining == 0)
        {
            Time.timeScale = 0f;
            EndgameUi.SetActive(true);
        }
    }
    void SpawnPrefab()
    {
        // Generate random x and y coordinates within the bounds of the BoxCollider
        Vector3 randomPosition = new Vector3(
            Random.Range(area.bounds.min.x, area.bounds.max.x),
            Random.Range(area.bounds.min.y, area.bounds.max.y),
            area.transform.position.z // Keep the z position the same as the spawner
        );

        // Instantiate the prefab at the random position
        Debug.Log("Spawned");
        Tobedeleted = Instantiate(prefabToSpawn, randomPosition, Quaternion.identity);
    }
    void TimesUP()
    {
        Destroy(Tobedeleted);
    }
    public void UpdatePercentageText()
    {
        // Calculate the spawn percentage
        float percentage = (float)KillCount / totalAllowedSpawns * 100;

        // Update the UI Text to show the percentage
        Percentage.text = string.Format("Killed Baterial: {0}/{1} ({2:F2}%)", KillCount, totalAllowedSpawns, percentage);
    }
}

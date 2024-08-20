using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TMPro;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public GameObject prefabToSpawn; // The prefab to spawn
    public float spawnInterval = 2f; // Time interval between spawns
    public float DeathInterval = 2f;
    private BoxCollider2D area; // The area within which to spawn the prefab
    public List<GameObject> Bacterials = new List<GameObject>(); // Use a List to hold spawned bacteria
    int bacterialcount = 0, bacterialspawned;
    public int KillCount, currentSpawnIndex, totalAllowedSpawns;
    public TextMeshProUGUI Percentage;
    private bool EndGamed = false;
    public GameObject crosshair;
    public GameObject EndgameUi;
    [SerializeField] private CountDown CountDown;

    void Start()
    {
        // Get the BoxCollider attached to the GameObject this script is attached to
        area = GetComponent<BoxCollider2D>();
        totalAllowedSpawns = Mathf.FloorToInt(CountDown.duration / spawnInterval);
        // Start the spawning process
        for(int i =0; i < 20; i++)
        {
            SpawnPrefab();
        }
        InvokeRepeating("SpawnPrefab", 0f, spawnInterval);
        InvokeRepeating("TimesUP", 0f, DeathInterval);
    }
    private void Update()
    {
        if (EndGamed == true)
        {
            CancelInvoke("SpawnPrefab");
            CancelInvoke("TimesUP");
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
        GameObject newBacterial = Instantiate(prefabToSpawn, randomPosition, Quaternion.identity);
        Bacterials.Add(newBacterial); // Add to the List
        bacterialspawned++;
    }

    void TimesUP()
    {
        // Spawn multiple bacteria based on the current number of spawned bacteria
        for (int i = 0; i < bacterialspawned; i++)
        {
            // Determine how many additional bacteria to spawn from each existing bacteria
            int additionalBacteriaCount = Random.Range(1, 4); // Randomly spawn 1 to 3 additional bacteria
            for (int j = 0; j < additionalBacteriaCount; j++)
            {
                if (Bacterials[i] != null)
                {
                    Vector3 spawnPosition = Bacterials[i].transform.position + (Vector3)(Random.insideUnitCircle * 0.5f); // Spread them slightly
                    GameObject newBacterial = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
                    Bacterials.Add(newBacterial); // Add to the List
                }
            }
        }
    }
    public void UpdatePercentageText()
    {
        // Calculate the spawn percentage
        float percentage = (float)KillCount / Bacterials.Count() * 100;
        Debug.Log(Bacterials.Count());
        // Update the UI Text to show the percentage
        Percentage.text = string.Format("Killed Baterial: {0}/{1} ({2:F2}%)", KillCount, Bacterials.Count(), percentage);
    }
    public void EndGame()
    {
        EndGamed = true;
        Time.timeScale = 1f;
        EndgameUi.SetActive(true);
    }
}

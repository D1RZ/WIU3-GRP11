using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFish : MonoBehaviour
{
    public Sprite graySprite; // Assign this in the Inspector
    public Sprite skeleSprite; // Assign this in the Inspector

    private State currentState;
    private Vector2 targetPosition;
    private Collider2D targetFood;

    [SerializeField] private Collider2D roamingArea;
    [SerializeField] private LayerMask foodLayer; // For efficiency reasons
    [SerializeField] private GameObject bigFishPrefab; // For spawning new big fish
    [SerializeField] private GameObject spawnFood; // food to spawn
    [SerializeField] private GameObject spawnArea; // Spawn area for food
    [SerializeField] private GameObject wastePrefab; // Prefab for the waste to be spawned

    private bool isWaiting = false;
    private bool onEatCooldown = false;
    private int smallFishEaten = 0; // Counter for spawning new big fish
    private float timeSinceLastEat = 0f; // Timer for tracking time since last eat

    [SerializeField] private float roamingSpeed = 1f; // Roaming speed of the big fish
    [SerializeField] private float eatingSpeed = 3f; // Burst speed when eating or chasing small fish
    [SerializeField] private float dieSpeed = 1f; // Speed the fish floats up at when it dies
    [SerializeField] private float detectionRadius = 10f; // detectionRadius for food
    [SerializeField] private float deathTime = 10f; // Time before the fish dies if no food is eaten

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        currentState = State.ROAM;
        PickRandomPoint();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
    }

    private enum State
    {
        ROAM,
        EAT
    }

    private void Update()
    {
        // Update timer
        if (currentState == State.ROAM)
        {
            timeSinceLastEat += Time.deltaTime;
            if (timeSinceLastEat >= deathTime)
            {
                Die();
                return;
            }
        }

        switch (currentState)
        {
            case State.ROAM:
                if (!isWaiting)
                    Roam();
                break;
            case State.EAT:
                Eat();
                break;
        }
        LookForward();
    }

    private void Roam()
    {
        DetectFood();

        float distanceToTarget = Vector2.Distance(transform.position, targetPosition);
        float currentSpeed = roamingSpeed;

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
        if (distanceToTarget <= 0.01f) // Start waiting when fish reach target position
            StartCoroutine(WaitBeforeNextMove());
    }

    private void Eat()
    {
        if (targetFood != null) // If food still exists
        {
            transform.position = Vector2.MoveTowards(transform.position, targetFood.transform.position, eatingSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetFood.transform.position) <= 0.5f) // When gets within range to eat food
            {
                Destroy(targetFood.gameObject);
                smallFishEaten++;
                DropWaste();
                if (smallFishEaten >= 3) // if ate 3 small fish, spawn new big fish
                {
                    SpawnNewBigFish();
                    smallFishEaten = 0;
                }
                StartCoroutine(EatCooldown());

                // Reset timer since food was eaten
                timeSinceLastEat = 0f;
                targetFood = null; // Reset targetFood to find new food
            }
        }
        else
        {
            // If targetFood got eaten, find new food
            DetectFood();
        }

        // If no food found at all, return to roaming state
        if (targetFood == null)
        {
            currentState = State.ROAM;
            PickRandomPoint();
        }
    }

    private void SpawnNewBigFish()
    {
        // Spawn new big fish at same position and rotation
        Instantiate(bigFishPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
    }

    private IEnumerator EatCooldown()
    {
        onEatCooldown = true;

        // Wait for a random time between 8 to 10 seconds
        float waitTime = Random.Range(8f, 10f);
        yield return new WaitForSeconds(waitTime);

        onEatCooldown = false;
        currentState = State.ROAM;
        PickRandomPoint();
    }

    private void DetectFood()
    {
        if (onEatCooldown) return;

        Collider2D[] foodInRange = Physics2D.OverlapCircleAll(transform.position, detectionRadius, foodLayer);
        if (foodInRange.Length > 0) // Food further than certain distance away
        {
            foreach (Collider2D food in foodInRange)
            {
                if (food.CompareTag("SmallFish"))
                {
                    targetFood = food; // Store the reference to the detected small fish
                    targetPosition = food.transform.position;
                    currentState = State.EAT;
                    break;
                }
            }
        }
    }

    private void LookForward()
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;    // Get direction of target position
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;    // Rotate fish to face direction it's moving in

        if (direction.x < 0) // if moving left
            transform.rotation = Quaternion.Euler(new Vector3(180, 0, -angle));    // Flip the sprite and adjust the angle
        else // if moving right
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));    // Don't flip sprite
    }

    private IEnumerator WaitBeforeNextMove()
    {
        isWaiting = true;

        // Wait for a random time between 0.2 to 1 seconds
        float waitTime = Random.Range(0.2f, 1f);
        yield return new WaitForSeconds(waitTime);

        // After waiting, pick a new random point to move to
        PickRandomPoint();
        isWaiting = false;
    }

    private IEnumerator WaitToDecompose()
    {
        yield return new WaitForSeconds(10f);

        // Check for nearby seaweed within a radius of 6f
        Collider2D[] seaweedInRange = Physics2D.OverlapCircleAll(transform.position, 6f, foodLayer);
        int seaweedSpawned = 0; // Counter to track how many seaweed have been spawned
        int maxSeaweedSpawn = 10; // Max number of seaweed that can spawn

        foreach (Collider2D seaweed in seaweedInRange)
        {
            if (seaweed.CompareTag("Seaweed"))
            {
                for (int i = 0; i < 1; i++) // how many seaweed to spawn per 1 detected seaweed
                {
                    if (seaweedSpawned >= maxSeaweedSpawn)
                        break;

                    Transform spawnTransform = spawnArea.transform;

                    // Get the fish's position and determine a random direction and distance within a 6f radius
                    Vector2 randomDirection = Random.insideUnitCircle.normalized;
                    float randomDistance = Random.Range(0.5f, 6f); // Choose a distance within the 6f radius
                    Vector2 potentialSpawnPosition = (Vector2)transform.position + randomDirection * randomDistance;

                    // Ensure the spawn position is within the bounds of the spawn area
                    float spawnX = Mathf.Clamp(
                        potentialSpawnPosition.x,
                        spawnTransform.position.x - spawnTransform.localScale.x / 2,
                        spawnTransform.position.x + spawnTransform.localScale.x / 2
                    );

                    float spawnY = Mathf.Clamp(
                        potentialSpawnPosition.y,
                        spawnTransform.position.y - spawnTransform.localScale.y / 2,
                        spawnTransform.position.y + spawnTransform.localScale.y / 2
                    );

                    Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnTransform.position.z);

                    Instantiate(spawnFood, spawnPosition, Quaternion.identity);
                    seaweedSpawned++;
                }
                if (seaweedSpawned >= maxSeaweedSpawn)
                    break;
            }
        }

        // Change to skeleton sprite
        spriteRenderer.sprite = skeleSprite;
        yield return new WaitForSeconds(1f);

        // Destroy the game object after spawning
        Destroy(gameObject);
    }

    private void PickRandomPoint()
    {
        if (roamingArea != null)
        {
            Bounds bounds = roamingArea.bounds;
            float x = Random.Range(bounds.min.x + 1f, bounds.max.x - 1f);
            float y = Random.Range(bounds.min.y + 1f, bounds.max.y);
            targetPosition = new Vector2(x, y);
        }
    }

    private void DropWaste()
    {
        // Start the coroutine to spawn waste after 10 seconds
        StartCoroutine(SpawnWaste());
    }

    private IEnumerator SpawnWaste()
    {
        // Wait for 10 seconds
        yield return new WaitForSeconds(10f);

        // Instantiate waste at the fish's current position
        Instantiate(wastePrefab, transform.position, Quaternion.identity);
    }

    private void Die()
    {
        currentState = State.ROAM; // Ensure state doesn't interfere with dying animation
        transform.rotation = Quaternion.identity; // Reset rotation
        StartCoroutine(WaitToDecompose());
    }

    private void OnDrawGizmos()
    {
        // Draw a green sphere around the fish to show detection radius
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
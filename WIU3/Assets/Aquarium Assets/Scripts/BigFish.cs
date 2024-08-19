using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFish : MonoBehaviour
{
    public Sprite graySprite;
    public Sprite skeleSprite;

    private State currentState;
    private Vector2 targetPosition;
    private Collider2D targetFood;

    [SerializeField] private Collider2D roamingArea;
    [SerializeField] private LayerMask foodLayer; // For efficiency reasons
    [SerializeField] private GameObject bigFishPrefab;
    [SerializeField] private GameObject spawnFood;
    [SerializeField] private GameObject spawnArea;
    [SerializeField] private GameObject wastePrefab;

    private bool isWaiting = false;
    private bool onEatCooldown = false;
    private int smallFishEaten = 0;
    private float timeSinceLastEat = 0f; // Timer for tracking time since last eat

    [SerializeField] private float roamingSpeed = 1f;
    [SerializeField] private float eatingSpeed = 3f; // Burst speed when chasing small fish
    [SerializeField] private float dieSpeed = 1f; // Speed of the fish sinking when it dies
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float starveTime = 40f;
    [SerializeField] private float lifeSpan = 60f;
    private float timeAlive;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        currentState = State.ROAM;
        PickRandomPoint();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component

        timeAlive = 0f;
    }

    private enum State
    {
        ROAM,
        EAT
    }

    private void Update()
    {
        // Update timer - for starving to death
        if (currentState == State.ROAM)
        {
            timeSinceLastEat += Time.deltaTime;
            if (timeSinceLastEat >= starveTime)
            {
                Die();
                return;
            }
        }

        // Update timer - for dying to old age
        timeAlive += Time.deltaTime;
        if (timeAlive >= lifeSpan)
        {
            Die();
            return;
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
                if (smallFishEaten >= 4) // if ate 4 small fish, spawn new big fish
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

        // Wait for a random time between 6 to 8 seconds
        float waitTime = Random.Range(5f, 6f);
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

        // Change to skeleton sprite
        spriteRenderer.sprite = skeleSprite;
        yield return new WaitForSeconds(1f);

        // Check for nearby seaweed within a radius of 3f
        Collider2D[] seaweedInRange = Physics2D.OverlapCircleAll(transform.position, 3f, foodLayer);
        int seaweedSpawned = 0; // Counter to track how many seaweed have been spawned
        int maxSeaweedSpawn = 5; // Max number of seaweed that can spawn

        foreach (Collider2D seaweed in seaweedInRange)
        {
            if (seaweed.CompareTag("Seaweed"))
            {
                for (int i = 0; i < 1; i++) // how many seaweed to spawn per 1 detected seaweed
                {
                    if (seaweedSpawned >= maxSeaweedSpawn)
                        break;

                    Transform spawnTransform = spawnArea.transform;

                    // Get the fish's position and determine a random direction and distance within a 3f radius
                    Vector2 randomDirection = Random.insideUnitCircle.normalized;
                    float randomDistance = Random.Range(0.5f, 3f); // Choose a distance within the 3f radius
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
        // Change the sprite to the gray one
        spriteRenderer.sprite = graySprite;

        // Reset rotation to default and flip sprite upside down
        transform.rotation = Quaternion.Euler(Vector3.zero);
        Vector3 scale = transform.localScale;
        scale.y = -Mathf.Abs(scale.y); // Flip vertically
        transform.localScale = scale;

        // Move downwards until it reaches the bottom of the roam area
        float botY = roamingArea.bounds.min.y;
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, botY), dieSpeed * Time.deltaTime);

        // Wait to decompose
        StartCoroutine(WaitToDecompose());
    }

    private void OnDrawGizmos()
    {
        // Draw a green sphere around the fish to show detection radius
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
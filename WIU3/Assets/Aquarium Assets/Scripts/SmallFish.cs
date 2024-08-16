using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallFish : MonoBehaviour
{
    public Sprite graySprite; // Assign this in the Inspector
    public Sprite skeleSprite; // Assign this in the Inspector

    private State currentState;
    private Vector2 targetPosition;
    private Collider2D targetFood;
    private SpriteRenderer spriteRenderer; // Store the SpriteRenderer component

    [SerializeField] private GameObject spawnFood;
    [SerializeField] private GameObject spawnArea;
    [SerializeField] private Collider2D roamingArea;
    [SerializeField] private LayerMask foodLayer;
    [SerializeField] private GameObject fish;

    private bool isWaiting = false;
    private bool onEatCooldown = false;
    private bool isDying = false;
    private int foodEaten = 0;
    private float timeSinceLastEat = 0f;

    [SerializeField] private float speed = 2f;
    [SerializeField] private float dieSpeed = 1f;
    [SerializeField] private float slowingDistance = 0.5f;
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float deathTime = 10f;

    private enum State
    {
        ROAM,
        EAT
    }

    private void Start()
    {
        // Store the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentState = State.ROAM;
        PickRandomPoint();
    }

    private void Update()
    {
        // Update timer - for starving to death
        timeSinceLastEat += Time.deltaTime;
        if (timeSinceLastEat >= deathTime)
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
        float currentSpeed = speed;

        if (distanceToTarget < slowingDistance)
            currentSpeed = Mathf.Lerp(0, speed, distanceToTarget / slowingDistance);    // Slow down the fish as it gets closer to the target

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
        if (distanceToTarget <= 0.01f) // Start waiting when fish reach target position
            StartCoroutine(WaitBeforeNextMove());
    }

    private void Eat()
    {
        if (targetFood != null) // If food still exists
        {
            Vector2 targetFoodPosition = targetFood.transform.position;

            // Move the fish towards the food position
            Vector2 newPosition = Vector2.MoveTowards(transform.position, targetFoodPosition, speed * Time.deltaTime);

            // Keep the fish within the bounds of the roaming area
            if (roamingArea != null)
            {
                Bounds bounds = roamingArea.bounds;
                newPosition.x = Mathf.Clamp(newPosition.x, bounds.min.x, bounds.max.x);
                newPosition.y = Mathf.Clamp(newPosition.y, bounds.min.y + 0.2f, bounds.max.y - 0.2f);
            }

            transform.position = newPosition;

            if (Vector2.Distance(transform.position, targetFoodPosition) <= 0.5f) // When gets within range to eat food
            {
                Destroy(targetFood.gameObject);
                foodEaten++;
                if (foodEaten >= 2) // if ate 2 food, spawn new fish
                {
                    spawnNewFish();
                    foodEaten = 0;
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

    private void spawnNewFish()
    {
        // Spawn new fish at same position and rotation
        Instantiate(fish, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
    }

    private IEnumerator EatCooldown()
    {
        onEatCooldown = true;

        // Wait for a random time between 3 to 5 seconds
        float waitTime = Random.Range(3f, 5f);
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
                if (food.CompareTag("Seaweed") || food.CompareTag("FoodPellet"))
                {
                    targetFood = food; // Store the reference to the detected food
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
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;    // Rotate fish to face direction its moving in

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
        yield return new WaitForSeconds(6f);

        // Change to skeleton sprite
        spriteRenderer.sprite = skeleSprite;
        yield return new WaitForSeconds(1f);
        // Check for nearby seaweed within a radius of 3f
        Collider2D[] seaweedInRange = Physics2D.OverlapCircleAll(transform.position, 3f, foodLayer);

        foreach (Collider2D seaweed in seaweedInRange)
        {
            if (seaweed.CompareTag("Seaweed"))
            {
                Debug.Log("pewee");
                // Spawn new objects within a 3f radius while still considering the bounds of the spawn area
                for (int i = 0; i < 1; i++) // Adjust this number to spawn more objects if needed
                {
                    // Access the Transform component of the spawnArea GameObject
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

                    // Instantiate the new object at the calculated position
                    Instantiate(spawnFood, spawnPosition, Quaternion.identity);
                }
            }
        }
        // Destroy gameobject
        Destroy(gameObject);
    }

    private IEnumerator WaitForSeconds(float sec)
    {
        yield return new WaitForSeconds(sec);
    }

    private void PickRandomPoint()
    {
        if (roamingArea != null)
        {
            Bounds bounds = roamingArea.bounds;
            float x = Random.Range(bounds.min.x + 1f, bounds.max.x - 1f);
            float y = Random.Range(bounds.min.y + 1f, bounds.max.y - 1f);
            targetPosition = new Vector2(x, y);
        }
    }

    private void Die()
    {
        isDying = true;

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
}
using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

public class SmallFish : MonoBehaviour
{
    private State currentState;
    private Vector2 targetPosition;
    private Collider2D targetFood;

    [SerializeField] private Collider2D roamingArea;
    [SerializeField] private LayerMask foodLayer; // For efficiency reasons
    [SerializeField] private GameObject fish; // For spawning new fish

    private bool isWaiting = false;
    private bool onEatCooldown = false;
    private int foodEaten = 0; // Counter for spawning new fish
    private float timeSinceLastEat = 0f; // Timer for tracking time since last eat

    [SerializeField] private float speed = 2f;
    [SerializeField] private float dieSpeed = 1f; // Speed the fish floats up at when it dies
    [SerializeField] private float slowingDistance = 0.5f; // Fish slow down when within this distance from target pos
    [SerializeField] private float detectionRadius = 10f; // detectionRadius for food
    [SerializeField] private float deathTime = 10f; // Time before the fish dies if no food is eaten

    private enum State
    {
        ROAM,
        EAT
    }

    private void Start()
    {
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
                if (food.CompareTag("FloatingPlant") || food.CompareTag("FoodPellet"))
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
        // Reset rotation to default and flip sprite upside down
        transform.rotation = Quaternion.Euler(Vector3.zero);
        Vector3 scale = transform.localScale;
        scale.y = -Mathf.Abs(scale.y); // Flip vertically
        transform.localScale = scale;

        // Move upwards until it reaches the top of the roam area
        float topY = roamingArea.bounds.max.y;
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, topY), dieSpeed * Time.deltaTime);

        // Destroy after 5 seconds
        Destroy(gameObject, 5f);
    }
}

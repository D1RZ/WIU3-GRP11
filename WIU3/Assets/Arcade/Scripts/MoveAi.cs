using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAi : MonoBehaviour
{
    public float speed = 2f;                  // Speed of the enemy
    public float movementInterval = 2f;       // Time interval to change direction
    public float movementRange = 5f;           // Range within which to move randomly

    private Vector2 randomDirection;           // Random direction to move
    private float timer;                       // Timer for changing direction
    [SerializeField] public BoxCollider2D Area;
    void Start()
    {
        // Initialize the random direction
        SetRandomDirection();
    }

    void Update()
    {
        // Move the enemy in the random direction
        transform.Translate(randomDirection * speed * Time.deltaTime);
        while (Area.OverlapPoint(transform.position))
        {
            SetRandomDirection();
        }
        // Update the timer
        timer += Time.deltaTime;

        // Check if it's time to change direction
        if (timer >= movementInterval)
        {
            SetRandomDirection();
            timer = 0;  // Reset timer
        }
    }

    void SetRandomDirection()
    {
        // Generate a random point within the movement range
        float randomX = Random.Range(-movementRange, movementRange);
        float randomY = Random.Range(-movementRange, movementRange);
        randomDirection = new Vector2(randomX, randomY).normalized; // Normalize to ensure consistent speed
    }
}

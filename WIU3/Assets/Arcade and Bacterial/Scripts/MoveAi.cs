using JetBrains.Annotations;
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
    [SerializeField] private BoxCollider2D Area;
    public GameObject Spawn;
    [SerializeField] private GameState game;
    public void Start()
    {
        if (game.GameStart)
        {
            Area = Spawn.GetComponent<BoxCollider2D>();
            // Initialize the random direction
            SetRandomDirection();
        }
    }

    public void Update()
    {
        if(game.GameStart)
        {
            // Move the enemy in the random direction
            transform.position = Vector2.MoveTowards(transform.position, randomDirection, speed * Time.deltaTime);

            // Update the timer
            timer += Time.deltaTime;

            // Check if it's time to change direction
            if (timer >= movementInterval)
            {
                SetRandomDirection();
                timer = 0;  // Reset timer
            }
        }
    }

    void SetRandomDirection()
    {
        Bounds bounds = Area.bounds;
        randomDirection.x = Mathf.Clamp(randomDirection.x, bounds.min.x, bounds.max.x);
        randomDirection.y = Mathf.Clamp(randomDirection.y, bounds.min.y, bounds.max.y);
    }
}

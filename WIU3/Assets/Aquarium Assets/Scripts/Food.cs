using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private float airSpeed = 3f;
    [SerializeField] private float waterSpeed = 0.5f;
    private float currentSpeed;
    private bool isInWater = false;

    private void Start()
    {
        currentSpeed = airSpeed;
        gameController.FoodSpawnReduceCondition();

        // Randomize between red, green, and blue
        RandomizeColor();
    }

    void Update()
    {
        if (gameController.GetGameStatus() == "End")
            return;

        // Move food downwards
        transform.Translate(Vector3.down * currentSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            if (!isInWater) // Check to ensure speed only changes once
            {
                isInWater = true;
                currentSpeed = waterSpeed;
            }
        }
    }

    private void RandomizeColor()
    {
        // Reference the SpriteRenderer component
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // Array of predefined colors (red, green, blue)
        Color[] colors = { Color.red, Color.green, Color.blue };

        // Select a random color from the array
        spriteRenderer.color = colors[Random.Range(0, colors.Length)];
    }
}
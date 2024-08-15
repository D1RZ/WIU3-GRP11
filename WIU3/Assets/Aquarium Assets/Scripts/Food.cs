using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private float airSpeed = 3f;
    [SerializeField] private float waterSpeed = 0.5f;
    private float currentSpeed;
    private bool isInWater = false;

    private void Start()
    {
        currentSpeed = airSpeed;
    }

    void Update()
    {
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
}

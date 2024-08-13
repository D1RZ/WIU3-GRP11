using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFood : MonoBehaviour
{
    public GameObject food;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;    // Get mouse position
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);   // Convert mouse position to world space
            mousePosition.z = 0; 

            Instantiate(food, mousePosition, Quaternion.identity);    // Spawn food
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFood : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    public GameObject food;
    void Update()
    {
        if (gameController.GetGameStatus() == "End")
            return;
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;    // Get mouse position
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);   // Convert mouse position to world space
            mousePosition.z = 0;

            Instantiate(food, mousePosition, Quaternion.identity);    // Spawn food
        }
    }
}

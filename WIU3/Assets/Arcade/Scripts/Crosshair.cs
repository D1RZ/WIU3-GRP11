using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Crosshair : MonoBehaviour
{
    private bool IsShooting = false;
    private GameObject baterialInsight;
    private bool isEnter = false;
    [SerializeField]private RandomSpawn Percentage;
    void Update()
    {
        IsShooting = Input.GetMouseButtonDown(0);
        // Get the mouse position in screen coordinates
        Vector3 mousePos = Input.mousePosition;

        // Convert the mouse position to world coordinates
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        // Set the Z position to the same as the sprite (2D game)
        mousePos.z = 0;

        // Center the sprite on the mouse cursor
        transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);

        if (isEnter && IsShooting)
        {
            Percentage.KillCount++;
            Percentage.UpdatePercentageText();
            Destroy(baterialInsight);
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("SpawnArea"))
        {
            baterialInsight = other.gameObject;
            isEnter = true;
            Debug.Log("Enter OnTriggerEnter2D");
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        baterialInsight = null;
        isEnter = false;
        Debug.Log("Exit OnTrigger");
    }
}

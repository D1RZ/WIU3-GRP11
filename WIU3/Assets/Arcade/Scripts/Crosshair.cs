using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Crosshair : MonoBehaviour
{
    private bool IsShooting = false;
    private List<GameObject> bacterialInsights = new List<GameObject>(); // List to track targetable objects
    private bool isEnter = false; // Check if the mouse is over a target
    [SerializeField] private CountDown countdown;
    [SerializeField] private AudioClip ShootClip;
    [SerializeField] AudioSettingsManager audioSettingsManager;
    [SerializeField] CropSoundManager cropSoundManager;
    [SerializeField] private RandomSpawn Percentage;
    [SerializeField] private GameState gameState;
    void Update()
    {
        if (gameState.GameStart)
        {
            IsShooting = Input.GetMouseButtonDown(0);

            // Get the mouse position in screen coordinates
            Vector3 mousePos = Input.mousePosition;

            // Convert the mouse position to world coordinates
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            mousePos.z = 0; // Set the Z position to the same as the sprite (2D game)

            // Center the sprite on the mouse cursor
            transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);

            if (countdown.timeRemaining != 0)
            {
                // Attempt to shoot if conditions are met
                TryShoot();
            }
        }
    }

    private void TryShoot()
    {
        if(IsShooting)
        {
            cropSoundManager.PlaySoundFXClip(ShootClip, transform, audioSettingsManager.GetSFX());
        }
        if (isEnter && IsShooting)
        {
            // Log the count of bacterialInsights before accessing it
            Debug.Log("Bacterial Count Before Shooting: " + bacterialInsights.Count);

            // Check if there are any objects in the list before accessing
            if (bacterialInsights.Count > 0)
            {
                try
                {
                    GameObject targetBacterial = bacterialInsights[0]; // Get the first object in the list
                    Percentage.KillCount++;
                    Percentage.UpdatePercentageText();
                    Destroy(targetBacterial);
                    bacterialInsights.RemoveAt(0); // Remove the destroyed object from the list
                    Debug.Log("Destroyed: " + targetBacterial.name);
                }
                catch (System.ArgumentOutOfRangeException ex)
                {
                    //Debug.LogError("ArgumentOutOfRangeException: " + ex.Message);
                }
            }
            else
            {
                Debug.LogWarning("No bacterial objects to destroy.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check for collision with objects that are not the spawn area
        if (!other.gameObject.CompareTag("SpawnArea"))
        {
            // Only add the object if it's not already in the list
            if (!bacterialInsights.Contains(other.gameObject))
            {
                bacterialInsights.Add(other.gameObject);
                isEnter = true;
                Debug.Log("Enter OnTriggerEnter2D: " + other.gameObject.name);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Remove the object from the list when it exits the collider
        if (bacterialInsights.Contains(collision.gameObject))
        {
            bacterialInsights.Remove(collision.gameObject);
            Debug.Log("Exit OnTrigger: " + collision.gameObject.name);
        }

        // If there are no more objects in the list, set isEnter to false
        if (bacterialInsights.Count == 0)
        {
            isEnter = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Continuously check for new objects entering the collider
        if (!other.gameObject.CompareTag("SpawnArea"))
        {
            // Only add the object if it's not already in the list
            if (!bacterialInsights.Contains(other.gameObject))
            {
                bacterialInsights.Add(other.gameObject);
                Debug.Log("Stay OnTriggerStay2D: " + other.gameObject.name);
            }
        }
    }
}
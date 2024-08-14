using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public string floatingPlantTag = "FloatingPlant"; // Tag for floating plants
    public string smallFishTag = "SmallFish"; // Tag for small fish

    private HashSet<Collider2D> floatingPlants = new HashSet<Collider2D>();
    private HashSet<Collider2D> smallFishes = new HashSet<Collider2D>();

    private bool isPressT;

    private void Start()
    {
        isPressT = Input.GetKey(KeyCode.T);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(floatingPlantTag))
        {
            floatingPlants.Add(other);
        }
        else if (other.CompareTag(smallFishTag))
        {
            smallFishes.Add(other);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(floatingPlantTag))
        {
            floatingPlants.Remove(other);
        }
        else if (other.CompareTag(smallFishTag))
        {
            smallFishes.Remove(other);
        }
    }

    void Update()
    {
        if (!isPressT)
        {
            Debug.Log("Pressed T");
            Debug.Log("Floating Plant Amount = " + floatingPlants.Count);
            Debug.Log("Small Fish Amount = " +  smallFishes.Count);
        }
    }
}

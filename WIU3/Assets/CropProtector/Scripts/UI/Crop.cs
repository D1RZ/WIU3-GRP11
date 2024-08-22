using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public float CropCurrentHealth;

    public float CropMaxHealth;

    // Start is called before the first frame update
    void Start()
    {
        CropCurrentHealth = 200f;
        CropMaxHealth = 200f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Activated");

        if (collision.gameObject.layer == 7)
        {
            Debug.Log("Activated2");

            if (collision.gameObject.tag == "Locust")
            {
                collision.gameObject.GetComponent<Locust>()._isEating = true;
            }
            else if(collision.gameObject.tag == "Mosquito")
            {
                collision.gameObject.GetComponent<Mosquito>()._isEating = true;
            }
            else if (collision.gameObject.tag == "Firefly")
            {
                collision.gameObject.GetComponent<Firefly>()._isEating = true;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    float maxHealth;

    float currentHealth;

    public void UpdateHealthBar()
    {
        if (transform.root.gameObject.tag == "Locust")
        {
            maxHealth = transform.root.gameObject.GetComponent<Locust>().MaxHealth;
            currentHealth = transform.root.gameObject.GetComponent<Locust>().Health;
        }
        else if(transform.root.gameObject.tag == "Mosquito")
        {
            maxHealth = transform.root.gameObject.GetComponent<Mosquito>().MaxHealth;
            currentHealth = transform.root.gameObject.GetComponent<Mosquito>().Health;
        }
        else if (transform.root.gameObject.tag == "Firefly")
        {
            maxHealth = transform.root.gameObject.GetComponent<Firefly>().MaxHealth;
            currentHealth = transform.root.gameObject.GetComponent<Firefly>().Health;
        }

        slider.value = currentHealth / maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthBar();
    }
}

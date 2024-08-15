using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hitbox")
        {
            if (collision.gameObject.transform.parent.gameObject.tag == "Locust")
            {
                Locust locust = collision.gameObject.transform.parent.gameObject.GetComponent<Locust>();
                locust.Health -= 25;
                Destroy(gameObject);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovementController : MonoBehaviour
{
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void MovePosition(Vector2 direction,float movementSpeed)
    {
        rb.velocity = direction * movementSpeed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovementController : MonoBehaviour
{
    public Rigidbody2D rb;

    public Rigidbody2D _PlayerRB
    {
        get
        {
            return rb;
        }
    }

    public void MovePosition(Vector2 direction,float movementSpeed)
    {
        rb.velocity = direction * movementSpeed;
    }
}

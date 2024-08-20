using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadePlayerMovement : MonoBehaviour
{
    private Animator animator;
    public float movspeed;
    float speedx, speedy;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        animator.SetBool("isMovingLeft", false);
        animator.SetBool("isMovingRight", false);
    }

    // Update is called once per frame
    void Update()
    {
        speedx = Input.GetAxisRaw("Horizontal") * movspeed;
        speedy = Input.GetAxisRaw("Vertical") * movspeed;
        rb.velocity = new Vector2 (speedx, speedy);

        if (Input.GetKey(KeyCode.A))
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isMovingRight", false);
            animator.SetBool("isMovingLeft", true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isMovingLeft", false);
            animator.SetBool("isMovingRight", true);
        }
        else
        {
            // Reset both booleans to return to idle
            animator.SetBool("isMovingLeft", false);
            animator.SetBool("isMovingRight", false);
            animator.SetBool("isIdle", true);
        }
    }
}

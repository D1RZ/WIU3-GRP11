using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadePlayerMovement : MonoBehaviour
{
    private Animator animator;
    public float movspeed;
    private Vector2 moveDirection;//To check which direction the player is moving
    float speedx, speedy;
    Rigidbody2D rb;
    [SerializeField] private PauseMenuUI Texting;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //Texting.TextPlay("This is the arcade in this place you can start playing our arcade games whilst playing the arcade games you will learn some topics regarding sustainability ARE YOU READY TO GET YOUR GAME ON!!!");
    }

    // Update is called once per frame
    void Update()
    {
        speedx = Input.GetAxisRaw("Horizontal") * movspeed;
        speedy = Input.GetAxisRaw("Vertical") * movspeed;
        rb.velocity = new Vector2(speedx, speedy);

        //GetMovement();
        CheckDirection();

        if (!Input.GetKey(KeyCode.W)
            && !Input.GetKey(KeyCode.A)
            && !Input.GetKey(KeyCode.S)
            && !Input.GetKey(KeyCode.D))
        {
            Debug.Log("is not moving");
            animator.SetBool("IsMoving", false);
        }
        else
        {
            Debug.Log("is moving");
            animator.SetBool("IsMoving", true);
        }

        animator.SetFloat("Horizontal", moveDirection.x);
        animator.SetFloat("Vertical", moveDirection.y);



        //if (Input.GetKey(KeyCode.A))
        //{
        //    animator.SetBool("isIdle", false);
        //    animator.SetBool("isMovingRight", false);
        //    animator.SetBool("isMovingLeft", true);
        //}
        //else if (Input.GetKey(KeyCode.D))
        //{
        //    animator.SetBool("isIdle", false);
        //    animator.SetBool("isMovingLeft", false);
        //    animator.SetBool("isMovingRight", true);
        //}
        //else
        //{
        //    // Reset both booleans to return to idle
        //    animator.SetBool("isMovingLeft", false);
        //    animator.SetBool("isMovingRight", false);
        //    animator.SetBool("isIdle", true);
        //}
    }

    private void GetMovement()
    {
        //speedx = Input.GetAxisRaw("Horizontal") * movspeed;
        //if (speedx != 0)
        //{
        //    speedy = Input.GetAxisRaw("Vertical") * movspeed;
        //}
        //rb.velocity = new Vector2(speedx, speedy);
    }
    private void CheckDirection()
    {
        ////Check up key
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    moveDirection += Vector2.up;
        //}
        //else if (Input.GetKeyUp(KeyCode.W))
        //{
        //    moveDirection -= Vector2.up;
        //}

        ////Check left key
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    moveDirection += Vector2.left;
        //}
        //else if (Input.GetKeyUp(KeyCode.A))
        //{
        //    moveDirection -= Vector2.left;
        //}

        ////Check down key
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    moveDirection += Vector2.down;
        //}
        //else if (Input.GetKeyUp(KeyCode.S))
        //{
        //    moveDirection -= Vector2.down;
        //}

        ////Check right key
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    moveDirection += Vector2.right;
        //}
        //else if (Input.GetKeyUp(KeyCode.D))
        //{
        //    moveDirection -= Vector2.right;
        //}

        if (Input.GetKeyDown(KeyCode.W))
        {
            moveDirection = Vector2.up;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            moveDirection = Vector2.left;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            moveDirection = Vector2.down;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            moveDirection = Vector2.right;
        }

        //Debug.Log(moveDirection);
    }
}

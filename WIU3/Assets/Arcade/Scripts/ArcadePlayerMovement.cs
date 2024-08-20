using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadePlayerMovement : MonoBehaviour
{
    public float movspeed;
    float speedx, speedy;
    Rigidbody2D rb;
    [SerializeField] private PauseMenuUI Texting;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Texting.TextPlay("This is the arcade in this place you can start playing our arcade games whilst playing the arcade games you will learn some topics regarding sustainability ARE YOU READY TO GET YOUR GAME ON!!!");
    }

    // Update is called once per frame
    void Update()
    {
        speedx = Input.GetAxisRaw("Horizontal") * movspeed;
        speedy = Input.GetAxisRaw("Vertical") * movspeed;
        rb.velocity = new Vector2 (speedx, speedy);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    public Animator anim { get; private set; }
    
    private PlayerController playerController;
    
    public PlayerController _PlayerController
    { get { return playerController; } }
    
    public GameObject Player { get; private set; }
    
    public virtual void Start()
    {
        Player = GameObject.Find("Player");
        playerController = Player.GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
    }
}

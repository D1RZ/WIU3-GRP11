using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    private PlayerController playerController;
    
    public PlayerController _PlayerController
    { get { return playerController; } }

    public GameObject Player;

    public GameManager gameManager;
    
    public virtual void Start()
    {
        if(Player != null)
        playerController = Player.GetComponent<PlayerController>();
    }
}

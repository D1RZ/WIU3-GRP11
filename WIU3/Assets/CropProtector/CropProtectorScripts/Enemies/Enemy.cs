using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    private PlayerController playerController;
    
    public PlayerController _PlayerController
    { get { return playerController; } }
    
    public GameObject Player { get; private set; }

    public GameManager gameManager;
    
    public virtual void Start()
    {
        Player = GameObject.Find("CropPlayer");
        if(Player != null)
        playerController = Player.GetComponent<PlayerController>();
        gameManager = GameObject.Find("CropGameManager").GetComponent<GameManager>();
    }
}

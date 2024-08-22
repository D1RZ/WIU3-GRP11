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

    public SpriteRenderer EnemyGraphic;
    public virtual void Start()
    {
        if(Player != null)
        playerController = Player.GetComponent<PlayerController>();

        EnemyGraphic = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    public Vector2 PerpendicularRightVector(Vector2 vector2)
    {
        return new Vector2(vector2.y,-vector2.x);
    }
    public Vector2 PerpendicularLeftVector(Vector2 vector2)
    {
        return new Vector2(-vector2.y,vector2.x);
    }
}

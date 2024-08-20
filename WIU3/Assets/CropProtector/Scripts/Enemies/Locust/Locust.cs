using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Locust : Enemy
{
    private State currentState;
    
    private float health;

    public float MaxHealth { get; private set; }

    public float Health
    {
        get
        {
            return health;
        }
    
        set
        {
            health = value;
        }
    }
    
    public enum State
    {
        Idle,
        Move,
        Chase,
        EnterAttack,
        Attack,
        ExitAttack,
        Hit,
        Eating
    }

    [SerializeField] private float movementSpeed;

    [SerializeField] private float dashSpeed;

    [SerializeField] private float retreatSpeed;

    [SerializeField] private float dashTimer;

    [SerializeField] private float chaseSpeed;

    [SerializeField] private AudioClip PlayerHurt;

    [SerializeField] private GameObject Crops;

    [SerializeField] private SpriteRenderer PlayerSprite;

    private Vector2 PrevPosition;

    private float totalDistance;

    private Vector2 DashDirection;

    public bool _isEating = false;

    private float _timeElapsed;
    
    public override void Start()
    {
        base.Start();
        currentState = State.Move;
        health = 100;
        MaxHealth = 100;
    }
    
    public void Update()
    {
        if (Crops.GetComponent<Crop>().CropCurrentHealth <= 0 || _PlayerController.playerData.health <= 0)
        {
            currentState = State.Idle;
            return;
        }

        if (health <= 0)
        {
            gameManager.LocustCount -= 1;
            Destroy(gameObject);
        }

        if (Player == null)
        return;

        if (_isEating)
        {
            currentState = State.Eating;
        }
        
        switch (currentState)
        {

          case State.Idle:

               break;

          case State.Move:
          
              if (!CheckForPlayer())
              {
                 if(transform.rotation.eulerAngles.z != 0)
                 {
                    Quaternion desiredAngle = Quaternion.Euler(0, 0, 0);
                    transform.rotation = desiredAngle;
                 }

                 transform.position = transform.position + Vector3.down * movementSpeed * Time.deltaTime;
              }
              else
              {
                  currentState = State.Chase;
              }
          
              if (Vector2.Distance(Player.transform.position, transform.position) <= 1.5f)
                  currentState = State.EnterAttack;
          
              break;
          
          case State.Chase:

               if (Vector2.Distance(Player.transform.position, transform.position) >= 3.5f) // player outside of chase range
               {
                   currentState = State.Move;
               }
               else
               {
                   RotateTowardsPlayer();

                   Vector2 direction = Player.transform.position - transform.position;
               
                   transform.position = (Vector2)transform.position + direction * chaseSpeed * Time.deltaTime;
               
                   if (Vector2.Distance(Player.transform.position, transform.position) <= 1.5f)
                       currentState = State.EnterAttack;
               }
          
              break;

          case State.EnterAttack:



                RotateTowardsPlayer();

                DashDirection = (Player.transform.position - transform.position).normalized;

                dashTimer -= Time.deltaTime;

                if (dashTimer <= 0)
                   currentState = State.Attack;

                break;
          
          case State.Attack:

                RotateTowardsPlayer();

                PrevPosition = transform.position;

                transform.position = (Vector2)transform.position + DashDirection * dashSpeed * Time.deltaTime;

                totalDistance += Vector2.Distance(PrevPosition, transform.position);
                
                if (totalDistance >= 1.8f && Vector3.Distance(Player.transform.position,transform.position) <= 2.0f)
                {
                    totalDistance = 0;
                    dashTimer = 0.5f;
                    currentState = State.EnterAttack;
                }
                else if(totalDistance >= 1.8f && Vector3.Distance(Player.transform.position, transform.position) >= 2.0f)
                {
                    totalDistance = 0;
                    dashTimer = 0.5f;
                    currentState = State.Chase;
                }

                break;

          case State.ExitAttack:

                RotateTowardsPlayer();

                PrevPosition = transform.position;

                Vector2 RetreatDirection = (transform.position - Player.transform.position).normalized;

                transform.position = (Vector2)transform.position + RetreatDirection * retreatSpeed * Time.deltaTime;

                totalDistance += Vector2.Distance(PrevPosition,transform.position);

                if (totalDistance >= 1.5f)
                {
                    currentState = State.EnterAttack;
                    totalDistance = 0;
                    dashTimer = 0.5f;
                }

                break;

            case State.Eating:

                _timeElapsed += Time.deltaTime;

                Crops.GetComponent<Crop>().CropCurrentHealth -= 0.01f * _timeElapsed;

                break;

            default:
          
              break;
        }
    }

    void RotateTowardsPlayer()
    {
        Vector2 DesiredDirection = (Player.transform.position - transform.position).normalized;

        float rotationZ = Mathf.Atan2(DesiredDirection.y, DesiredDirection.x) * Mathf.Rad2Deg + 90f;
        Quaternion targetAngle = Quaternion.Euler(0, 0, rotationZ);

        transform.rotation = targetAngle;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Crops.GetComponent<Crop>().CropCurrentHealth <= 0 || _PlayerController.playerData.health <= 0)
            return;

        if (collision.gameObject.layer == 6 && currentState == State.Attack)
        {
            totalDistance = 0;
            dashTimer = 0.5f;
            _PlayerController.playerData.health -= 20;
            PlayerSprite.color = Color.red;
            CropSoundManager.instance.PlaySoundFXClip(PlayerHurt,_PlayerController.gameObject.transform,AudioSettingsManager.instance.GetSFX());
            currentState = State.ExitAttack;
        }

    }

    bool CheckForPlayer()
    {
        return Vector2.Distance(Player.transform.position, transform.position) <= 2.25f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Mosquito : Enemy
{
    public State currentState;

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
        Attack,
        Hit,
        Eating
    }

    [SerializeField] private float movementSpeed;

    [SerializeField] private GameObject Crops;

    [SerializeField] private SpriteRenderer PlayerSprite;

    [SerializeField] private float AcidFireRate;

    [SerializeField] private GameObject acid;

    [SerializeField] private float acidMovementSpeed;

    [SerializeField] private Transform AcidLaunchPoint;

    [SerializeField] private GameObject PlayerPivot;

    private float HitTimer;

    public bool _isEating = false;

    private float _timeElapsed;

    private float FinalMovementSpeed;

    private bool EnterHit = false;

    private float InstantiatedAcidTime;

    private GameObject _lastSpawnedAcid;

    public override void Start()
    {
        base.Start();
        currentState = State.Move;
        health = 100;
        MaxHealth = 100;
        FinalMovementSpeed = movementSpeed + Random.Range(-0.5f, 0.5f);
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
            gameManager.MosquitoCount -= 1;
            Destroy(gameObject);
        }

        if (Player == null)
            return;

        if (_isEating)
        {
            currentState = State.Eating;
        }

        Debug.Log(currentState);

        switch (currentState)
        {

            case State.Idle:

                break;

            case State.Move:

                if (!CheckForPlayer())
                {
                    if (transform.rotation.eulerAngles.z != 0)
                    {
                        Quaternion desiredAngle = Quaternion.Euler(0, 0, 0);
                        transform.rotation = desiredAngle;
                    }

                    Debug.Log(FinalMovementSpeed);

                    transform.position = transform.position + Vector3.down * FinalMovementSpeed * Time.deltaTime;
                }
                else
                {
                    currentState = State.Attack;
                }

                break;

            case State.Attack:

                RotateTowardsPlayer();

                if (Vector2.Distance(Player.transform.position, transform.position) <= 3f)
                    ShootAtPlayer();
                else
                    currentState = State.Move;

                break;

            case State.Eating:

                _timeElapsed += Time.deltaTime;

                Crops.GetComponent<Crop>().CropCurrentHealth -= 0.01f * _timeElapsed;

                break;

            case State.Hit:

                if (HitTimer > 0)
                    HitTimer -= Time.deltaTime;

                if (!EnterHit)
                {
                    EnemyGraphic.color = Color.red;
                    HitTimer = 0.2f;
                    EnterHit = true;
                }

                if (HitTimer < 0)
                {
                    EnemyGraphic.color = Color.white;
                    EnterHit = false;
                    currentState = State.Move;
                }

                break;

            default:

                break;
        }
    }

    void RotateTowardsPlayer()
    {
        Vector2 DesiredDirection = (PlayerPivot.transform.position - transform.position).normalized;

        float rotationZ = Mathf.Atan2(DesiredDirection.y, DesiredDirection.x) * Mathf.Rad2Deg + 90f;
        Quaternion targetAngle = Quaternion.Euler(0, 0, rotationZ);

        transform.rotation = targetAngle;
    }

    void ShootAtPlayer()
    {
        Vector2 DesiredProjectileDirection = (PlayerPivot.transform.position - transform.position).normalized;

        float rotationZ = Mathf.Atan2(DesiredProjectileDirection.y,DesiredProjectileDirection.x) * Mathf.Rad2Deg;
        Quaternion targetAngle = Quaternion.Euler(0, 0, rotationZ);

        if (Time.time >= InstantiatedAcidTime + AcidFireRate)
        {
            _lastSpawnedAcid = Instantiate(acid,AcidLaunchPoint.position,targetAngle);

            if (_lastSpawnedAcid != null)
            {
                Rigidbody2D bulletRB = _lastSpawnedAcid.GetComponent<Rigidbody2D>();
                bulletRB.velocity = DesiredProjectileDirection * acidMovementSpeed;
                InstantiatedAcidTime = Time.time;
                //CropSoundManager.instance.PlaySoundFXClip(ARShotAudio, transform, audioSettingsManager.GetSFX());
            }
        }
    }

    bool CheckForPlayer()
    {
        return Vector2.Distance(Player.transform.position, transform.position) <= 2.25f;
    }
}

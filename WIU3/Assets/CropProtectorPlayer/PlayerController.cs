using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private MovementController movementController;

    [SerializeField] private PlayerData playerData;

    [SerializeField] private Camera camera;

    [SerializeField] private Transform GunPivot;

    [SerializeField] private SpriteRenderer GunSpriteRenderer;

    [SerializeField] private GameObject GunSprite;

    [SerializeField] private SpriteRenderer playerSpriteRenderer;

    [SerializeField] private Transform GunTip;

    [SerializeField] private GameObject bullet;

    [SerializeField] private float GunFireRate;

    [SerializeField] private float bulletMovementSpeed;

    Vector2 mousePosition;

    float rotZ;

    float ShotBulletTime;

    bool canShoot = true;

    bool hasShot = false;

    private GameObject _lastSpawnedBullet;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 direction = new Vector2(horizontal, vertical);
        mousePosition = camera.ScreenToWorldPoint(Input.mousePosition); // converts position of mouse on screen to world coordinates
        movementController.MovePosition(direction, playerData.movementSpeed);

        if(Input.GetMouseButtonDown(0) && canShoot)
        {
            ShotBulletTime = Time.time;
            canShoot = false;
            hasShot = true;
        }

        if (Time.time >= ShotBulletTime + GunFireRate && !canShoot)
            canShoot = true;
    }
    
    private void FixedUpdate()
    {
        Vector2 lookDir = mousePosition - (Vector2)GunPivot.position;
        rotZ = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        Quaternion target = Quaternion.Euler(0, 0, rotZ);

        GunPivot.rotation = target;

        if (rotZ < 89 && rotZ > -89)
        {
            if (GunSpriteRenderer.flipY)
            {
                GunSprite.transform.localPosition = new Vector3(0.403f,0.148f,GunSprite.transform.localPosition.z);
                GunTip.transform.localPosition = new Vector3(0.86f,-0.109f,GunTip.transform.localPosition.z);
                playerSpriteRenderer.flipX = false;
                GunSpriteRenderer.flipY = false;
            }
        }
        else
        {
            if (!GunSpriteRenderer.flipY)
            {
                GunSprite.transform.localPosition = new Vector3(0.4f,-0.482f,GunSprite.transform.localPosition.z);
                GunTip.transform.localPosition = new Vector3(0.883f,-0.253f, GunTip.transform.localPosition.z);
                playerSpriteRenderer.flipX = true;
                GunSpriteRenderer.flipY = true;
            }
        }

        if(hasShot)
        {
            _lastSpawnedBullet = Instantiate(bullet,GunTip.position,Quaternion.identity);
            if (_lastSpawnedBullet != null)
            {
                Rigidbody2D bulletRB = _lastSpawnedBullet.GetComponent<Rigidbody2D>();
                bulletRB.velocity = lookDir * bulletMovementSpeed;
                Debug.Log(bulletRB.velocity);
                hasShot = false;
            }
        }
    }
}

using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private MovementController movementController;

    public PlayerData playerData;

    [SerializeField] private Camera camera;

    [SerializeField] private Transform GunPivot;

    [SerializeField] private SpriteRenderer GunSpriteRenderer;

    [SerializeField] private GameObject GunSprite;

    [SerializeField] private SpriteRenderer playerSpriteRenderer;

    [SerializeField] private Transform GunTip;

    [SerializeField] private GameObject bullet;

    [SerializeField] private float GunFireRate;

    [SerializeField] private float bulletMovementSpeed;

    [SerializeField] private Transform GunCollider;

    [SerializeField] TextMeshProUGUI bannerText;

    [SerializeField] GameObject MakeScreenDarkerPanel;

    [SerializeField] GameObject EndGameUI;

    Vector2 mousePosition;

    float rotZ;

    float ShotBulletTime;

    bool canShoot = true;

    bool hasShot = false;

    private GameObject _lastSpawnedBullet;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        playerData.health = 100;
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerData.health <= 0)
            return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 direction = new Vector2(horizontal, vertical);
        mousePosition = camera.ScreenToWorldPoint(Input.mousePosition); // converts position of mouse on screen to world coordinates

        if (Mathf.Abs(horizontal) < 0.0001f && Mathf.Abs(vertical) < 0.0001f)
        {
            animator.SetBool("idle", true);
            animator.SetBool("move", false);
        }
        else
        {
            animator.SetBool("move", true);
            animator.SetBool("idle", false);
        }

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
                GunCollider.transform.localPosition = new Vector3(0,0,GunCollider.transform.localPosition.z);
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
                GunCollider.transform.localPosition = new Vector3(0.03f,1.8f, GunCollider.transform.localPosition.z);
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

    void OpenEndGame()
    {
        bannerText.text = "Better luck next time!";

        MakeScreenDarkerPanel.SetActive(true);

        EndGameUI.SetActive(true);
    }
}

using System.Collections.Generic;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField] private MovementController movementController;

    public PlayerData playerData;

    [SerializeField] private Camera camera;

    [SerializeField] private Transform GunPivot;

    [SerializeField] private SpriteRenderer GunSpriteRenderer;

    [SerializeField] private GameObject GunSprite;

    [SerializeField] private SpriteRenderer playerSpriteRenderer;

    [SerializeField] private Transform PistolBulletSpawn;

    [SerializeField] private List<Transform> ShotgunBulletSpawn;

    [SerializeField] private Transform ARBulletSpawn;

    [SerializeField] private GameObject bullet;

    [SerializeField] private float GunFireRate;

    [SerializeField] private float ARFireRate;

    [SerializeField] private float bulletMovementSpeed;

    [SerializeField] private float ARbulletMovementSpeed;

    [SerializeField] private Transform GunCollider;

    [SerializeField] TextMeshProUGUI bannerText;

    [SerializeField] GameObject MakeScreenDarkerPanel;

    [SerializeField] GameObject EndGameUI;

    [SerializeField] GameObject audioSettingsPanel;

    [SerializeField] AudioClip PistolShotAudio;

    [SerializeField] AudioClip ShotgunShotAudio;

    [SerializeField] AudioClip ARShotAudio;

    [SerializeField] Sprite AssaultRifleSprite;

    [SerializeField] Sprite PistolSprite;

    [SerializeField] Sprite ShotgunSprite;

    [SerializeField] float GunSwapDelay;

    [SerializeField] AudioClip WeaponSwapAudio;

    [SerializeField] AudioSettingsManager audioSettingsManager;

    public enum Gun
    {
        Pistol,
        Shotgun,
        AR
    }

    public Gun currentEquippedGun;

    Vector2 mousePosition;

    float rotZ;

    float ShotBulletTime;

    bool canShoot = true;

    bool hasShot = false;

    private GameObject _lastSpawnedPistolBullet;

    private GameObject[] _lastSpawnedShotgunBullet;

    private GameObject _lastSpawnedARBullet;

    private Animator animator;

    float lastMouseScrollTime;

    bool hasScrolled;

    bool isShooting = false;

    bool InstantiatedBullet = false;

    float InstantiatedBulletTime;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        playerData.health = 100;
        animator = gameObject.GetComponent<Animator>();
        currentEquippedGun = Gun.Pistol;
        _lastSpawnedShotgunBullet = new GameObject[3]; // initialises array with capacity of 3
        canShoot = true;
        InstantiatedBulletTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerData.health <= 0 || PauseMenuUI.instance.GameIsPaused)
            return;

        if (Input.GetKeyDown(KeyCode.I) && audioSettingsPanel.active == false)
            audioSettingsPanel.SetActive(true);
        else if (Input.GetKeyDown(KeyCode.I) && audioSettingsPanel.active == true)
            audioSettingsPanel.SetActive(false);

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 direction = new Vector2(horizontal, vertical);
        mousePosition = camera.ScreenToWorldPoint(Input.mousePosition); // converts position of mouse on screen to world coordinates

        Vector2 lookDir = mousePosition - (Vector2)GunPivot.position;
        rotZ = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        Quaternion target = Quaternion.Euler(0, 0, rotZ);

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

        if (Input.mouseScrollDelta.y > 0 && currentEquippedGun != Gun.Pistol && !hasScrolled)
        {
            if (currentEquippedGun == Gun.Shotgun)
            {
                CropSoundManager.instance.PlaySoundFXClip(WeaponSwapAudio, transform, audioSettingsManager.GetSFX());
                GunSpriteRenderer.sprite = PistolSprite;
                currentEquippedGun = Gun.Pistol;
                lastMouseScrollTime = Time.time;
                hasScrolled = true;
            }
            else if (currentEquippedGun == Gun.AR)
            {
                CropSoundManager.instance.PlaySoundFXClip(WeaponSwapAudio, transform, audioSettingsManager.GetSFX());
                GunSpriteRenderer.sprite = ShotgunSprite;
                currentEquippedGun = Gun.Shotgun;
                lastMouseScrollTime = Time.time;
                hasScrolled = true;
            }
        }
        else if (Input.mouseScrollDelta.y < 0 && currentEquippedGun != Gun.AR && !hasScrolled)
        {
            if (currentEquippedGun == Gun.Pistol)
            {
                CropSoundManager.instance.PlaySoundFXClip(WeaponSwapAudio, transform, audioSettingsManager.GetSFX());
                GunSpriteRenderer.sprite = ShotgunSprite;
                currentEquippedGun = Gun.Shotgun;
                lastMouseScrollTime = Time.time;
                hasScrolled = true;
            }
            else if (currentEquippedGun == Gun.Shotgun)
            {
                CropSoundManager.instance.PlaySoundFXClip(WeaponSwapAudio, transform, audioSettingsManager.GetSFX());
                GunSpriteRenderer.sprite = AssaultRifleSprite;
                currentEquippedGun = Gun.AR;
                lastMouseScrollTime = Time.time;
                hasScrolled = true;
            }
        }

        if (Time.time > lastMouseScrollTime + GunSwapDelay && hasScrolled)
            hasScrolled = false;

        if (currentEquippedGun == Gun.Pistol || currentEquippedGun == Gun.Shotgun) // shotgun and pistol both use GetMouseButtonDown
        {
            if (Input.GetMouseButtonDown(0) && canShoot)
            {
                ShotBulletTime = Time.time;
                canShoot = false;
                hasShot = true;
                if (currentEquippedGun == Gun.Pistol) // plays pistol gunshot sound
                    CropSoundManager.instance.PlaySoundFXClip(PistolShotAudio, transform,audioSettingsManager.GetSFX());

                if (currentEquippedGun == Gun.Shotgun)
                    CropSoundManager.instance.PlaySoundFXClip(ShotgunShotAudio,transform,audioSettingsManager.GetSFX());
            }

            if (Time.time >= ShotBulletTime + GunFireRate && !canShoot)
                canShoot = true;
        }
        else if (currentEquippedGun == Gun.AR) // AR uses GetMouseButton
        {
            if (Input.GetMouseButton(0))
            {
                if (Time.time >= InstantiatedBulletTime + ARFireRate)
                {
                    _lastSpawnedARBullet = Instantiate(bullet, ARBulletSpawn.position,target);

                    if (_lastSpawnedARBullet != null)
                    {
                        Rigidbody2D bulletRB = _lastSpawnedARBullet.GetComponent<Rigidbody2D>();
                        bulletRB.velocity = lookDir * ARbulletMovementSpeed;
                        InstantiatedBulletTime = Time.time;
                        CropSoundManager.instance.PlaySoundFXClip(ARShotAudio, transform, audioSettingsManager.GetSFX());
                    }
                }
            }
            else
            {
                isShooting = false;
            }
        }
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
                GunSprite.transform.localPosition = new Vector3(0.403f, 0.148f, GunSprite.transform.localPosition.z);
                PistolBulletSpawn.transform.localPosition = new Vector3(0.86f, -0.109f, PistolBulletSpawn.transform.localPosition.z);
                GunCollider.transform.localPosition = new Vector3(0, 0, GunCollider.transform.localPosition.z);
                playerSpriteRenderer.flipX = false;
                GunSpriteRenderer.flipY = false;
            }
        }
        else
        {
            if (!GunSpriteRenderer.flipY)
            {
                GunSprite.transform.localPosition = new Vector3(0.4f, -0.482f, GunSprite.transform.localPosition.z);
                PistolBulletSpawn.transform.localPosition = new Vector3(0.883f, -0.253f, PistolBulletSpawn.transform.localPosition.z);
                GunCollider.transform.localPosition = new Vector3(0.03f, 1.8f, GunCollider.transform.localPosition.z);
                playerSpriteRenderer.flipX = true;
                GunSpriteRenderer.flipY = true;
            }
        }

        if (hasShot)
        {
            if (currentEquippedGun == Gun.Pistol)
            {
                _lastSpawnedPistolBullet = Instantiate(bullet, PistolBulletSpawn.position,target);
                if (_lastSpawnedPistolBullet != null)
                {
                    Rigidbody2D bulletRB = _lastSpawnedPistolBullet.GetComponent<Rigidbody2D>();
                    bulletRB.velocity = lookDir * bulletMovementSpeed;
                    hasShot = false;
                }
            }
            else if (currentEquippedGun == Gun.Shotgun)
            {
                for (int i = 0; i < ShotgunBulletSpawn.Count; i++)
                {
                    _lastSpawnedShotgunBullet[i] = Instantiate(bullet, ShotgunBulletSpawn[i].position,target);
                    if (_lastSpawnedShotgunBullet[i] != null)
                    {
                        Rigidbody2D bulletRB = _lastSpawnedShotgunBullet[i].GetComponent<Rigidbody2D>();
                        bulletRB.velocity = lookDir * bulletMovementSpeed;
                    }
                }
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


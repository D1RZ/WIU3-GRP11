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

    Vector2 mousePosition;

    float rotZ;

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
                GunSprite.transform.localPosition = new Vector3(0.403f,0.148f, GunSprite.transform.localPosition.z);
                playerSpriteRenderer.flipX = false;
                GunSpriteRenderer.flipY = false;
            }
        }
        else
        {
            if (!GunSpriteRenderer.flipY)
            {
                GunSprite.transform.localPosition = new Vector3(0.4f,-0.482f,GunSprite.transform.localPosition.z);
                playerSpriteRenderer.flipX = true;
                GunSpriteRenderer.flipY = true;
            }
        }
    }
}

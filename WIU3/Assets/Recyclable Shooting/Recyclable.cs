using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recyclable : MonoBehaviour
{
    [SerializeField] float maxPower;
    [SerializeField] float maxPowerSpeed;
    float shotPower;
    private Rigidbody2D rb;
    [SerializeField] private RecyclableData data;
    public GameObject SpriteImage;

    private Vector2 AimPosition;
    private bool isThrown;

    private PhysicsScene2D sceneMainPhysics;
    private PhysicsScene2D scenePredictionPhysics;

    // Start is called before the first frame update
    void Start()
    {
        // Get the bounds of the parent and child
        SpriteRenderer parentRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer childRenderer = SpriteImage.GetComponent<SpriteRenderer>();

        transform.name = data.recyclableName;
        childRenderer.sprite = data.Image;
        rb = GetComponent<Rigidbody2D>();

        // Calculate the scale factor
        Vector3 parentSize = parentRenderer.bounds.size;
        Vector3 childSize = childRenderer.bounds.size;
        Debug.Log(childSize);

        float scaleX = parentSize.x / childSize.x;
        float scaleY = parentSize.y / childSize.y;

        // Apply the scale to the child
        SpriteImage.transform.localScale = new Vector3(scaleX, scaleY, 1f);

        //make it so that it does not fall first
        rb.gravityScale = 0.0f;

        AimPosition = Vector2.zero;
        isThrown = false;
        shotPower = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //get normalized position of ball pos to mouse pos
        if (!isThrown)
        {
            AimPosition = (mousePosition() - transform.position).normalized;
            //Debug.Log(AimPosition);
        }

        if (Input.GetMouseButton(0) && !isThrown)
        {
            // Increase the shot power over time
            shotPower = Mathf.Lerp(shotPower, maxPower, maxPowerSpeed * Time.deltaTime);
            Debug.Log(shotPower);
        }
        if (Input.GetMouseButtonUp(0) && !isThrown)
        {
            //Freeze any gravity first
            //rb.velocity = Vector2.zero;

            //give back gravity
            rb.gravityScale = 1.0f;

            //Check where the mouse is aiming
            AimPosition = (mousePosition() - transform.position).normalized;

            //Shot in mouse direction
            rb.AddForce(AimPosition * shotPower, ForceMode2D.Impulse);

            //Reset variables and make sure recyclable cannot be thrown again
            shotPower = 0;
            isThrown = true;
        }

    }

    private Vector3 mousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recyclable : MonoBehaviour
{
    [SerializeField] float maxPower;
    [SerializeField] float maxPowerSpeed;
    [SerializeField] private float despawnDelay = 2.0f;    // Time in seconds before the object is despawned
    float shotPower;

    private GameObject GameManager;

    private Rigidbody2D rb;
    public RecyclableData data;
    public GameObject SpriteImage;

    private bool properlyScored;

    private PhysicsScene2D sceneMainPhysics;
    private PhysicsScene2D scenePredictionPhysics;

    // Start is called before the first frame update
    void Start()
    {
        properlyScored = false;
        GameManager = GameObject.Find("Score");

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
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        properlyScored = transform.position.y > collision.gameObject.transform.position.y;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(properlyScored)
        { 
            Transform Hoop = collision.gameObject.transform.parent;
            
            //If recyclable goes into the correct hoop
            if (Hoop.GetComponent<Hoop>().type == data.type)
            {
                GameManager.GetComponent<RecyclingGameManager>().addScore();
            }
            else
            {
                GameManager.GetComponent<RecyclingGameManager>().minusScore();
            }
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object has the tag "Ground"
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // Call the Despawn method after a delay
            Invoke("Despawn", despawnDelay);
        }
    }

    private Vector3 mousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void Despawn()
    {
        // Destroy the game object
        Destroy(gameObject);
    }
}

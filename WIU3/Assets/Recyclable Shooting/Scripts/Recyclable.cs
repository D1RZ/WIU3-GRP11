using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recyclable : MonoBehaviour
{
    [SerializeField] private float maxPower;
    [SerializeField] private float maxPowerSpeed;
    [SerializeField] private float despawnDelay = 2.0f;    // Time in seconds before the object is despawned
    float shotPower;

    private GameObject GameManager;

    private Rigidbody2D rb;
    public RecyclableData data;
    public GameObject SpriteImage;

    private bool properlyScored;

    [SerializeField] private GameObject starEffect;

    public float minPitch = 0.5f;    // Minimum pitch value
    public float maxPitch = 3.0f;    // Maximum pitch value
    public float maxVelocity = 20.0f; // Maximum velocity to map
    private AudioSource sfxAudioSrc;
    [SerializeField] private AudioClip HoopAudioClip;
    [SerializeField] private AudioClip GroundAudioClip;
    [SerializeField] private GameObject sfxObject;

    // Start is called before the first frame update
    void Start()
    {
        sfxAudioSrc = GetComponent<AudioSource>();

        properlyScored = false;
        GameManager = GameObject.Find("GameManager");

        // Get the bounds of the parent and child
        SpriteRenderer parentRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer childRenderer = SpriteImage.GetComponent<SpriteRenderer>();

        transform.name = data.recyclableName;
        childRenderer.sprite = data.Image;
        rb = GetComponent<Rigidbody2D>();

        // Calculate the scale factor
        Vector3 parentSize = parentRenderer.bounds.size;
        Vector3 childSize = childRenderer.bounds.size;

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
        //Check if recyclable is above object
        properlyScored = transform.position.y > collision.gameObject.transform.position.y;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (properlyScored && collision.gameObject.layer == LayerMask.NameToLayer("Hoop"))
        {

            //GameObject newHoopSFX = Instantiate(sfxObject, transform);
            //AudioSource audioSource = newHoopSFX.GetComponent<AudioSource>();

            Transform Hoop = collision.gameObject.transform.parent;
            
            //If recyclable goes into the correct hoop
            if (Hoop.GetComponent<Hoop>().type == data.type)
            {
                Debug.Log("Correct bin audio played");
                //audioSource.PlayOneShot(correctBinAudioClip, 1.0f);

                GameManager.GetComponent<RecyclingGameManager>().addScore();

                //get parent of the hoop
                Transform hoop = collision.gameObject.transform.parent;

                Vector2 offset = new Vector2(0.0f, -0.7f);

                GameObject newStarEffect = Instantiate(starEffect, hoop);

                // Set the local position with the offset applied
                newStarEffect.transform.localPosition = offset;
            }
            else
            {
                Debug.Log("Wrong bin audio played");

                //audioSource.PlayOneShot(wrongBinAudioClip, 1.0f);

                GameManager.GetComponent<RecyclingGameManager>().minusScore();
            }
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the velocity magnitude of the object
        float velocity = rb.velocity.magnitude;

        // Map the velocity to the pitch range
        float pitch = Mathf.Lerp(minPitch, maxPitch, velocity / maxVelocity);

        // Set the pitch of the AudioSource
        sfxAudioSrc.pitch = pitch;

        // Check if the collided object has the tag "Ground"
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")
        || collision.gameObject.layer == LayerMask.NameToLayer("Recyclable"))
        {
            sfxAudioSrc.PlayOneShot(GroundAudioClip, 1.0f);

            // Call the Despawn method after a delay
            Invoke("Despawn", despawnDelay);
        }
        else
        {
            //recyclable has hit the hoop
            sfxAudioSrc.PlayOneShot(HoopAudioClip, 1.0f);

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

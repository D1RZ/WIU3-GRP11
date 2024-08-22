using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopTrigger : MonoBehaviour
{
    [SerializeField] private GameObject GameManager;
    [SerializeField] private GameObject starEffect;

    [SerializeField] private AudioClip correctBinAudioClip;
    [SerializeField] private AudioClip correctBinDoublePointsAudioClip;
    [SerializeField] private AudioClip wrongBinAudioClip;
    private GameObject Hoop;
    private AudioSource audioSource;
    private bool properlyScored;


    // Start is called before the first frame update
    void Start()
    {
        Hoop = transform.parent.gameObject;
        audioSource = Hoop.GetComponent<AudioSource>();
        properlyScored = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if Hoop is below object
        properlyScored = transform.position.y < collision.gameObject.transform.position.y;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Recyclable")
            && properlyScored)
        {
            //object gets into the correct hoop
            if (collision.gameObject.GetComponent<Recyclable>().data.type == Hoop.GetComponent<Hoop>().type)
            {
                GameManager.GetComponent<RecyclingGameManager>().addScore();

                Vector2 offset = new Vector2(0.0f, -0.7f);

                GameObject newStarEffect = Instantiate(starEffect, Hoop.transform);

                // Set the local position with the offset applied
                newStarEffect.transform.localPosition = offset;

                if (GameManager.GetComponent<RecyclingGameManager>().doublePoints)
                {
                    audioSource.PlayOneShot(correctBinDoublePointsAudioClip, 1.0f);
                }
                else
                {
                    audioSource.PlayOneShot(correctBinAudioClip, 1.0f);
                }
            }
            //object gets into the wrong hoop
            else
            {
                GameManager.GetComponent<RecyclingGameManager>().minusScore();

                audioSource.PlayOneShot(wrongBinAudioClip, 1.0f);
            }

            Destroy(collision.gameObject);
        }
    }
}

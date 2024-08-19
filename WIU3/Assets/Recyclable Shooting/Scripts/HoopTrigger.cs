using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip correctBinAudioClip;
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
            if (collision.gameObject.GetComponent<Recyclable>().data.type == Hoop.GetComponent<Hoop>().type)
            {
                audioSource.clip = correctBinAudioClip;
            }
            else
            {
                audioSource.clip = wrongBinAudioClip;
            }
        }

        audioSource.Play();
    }
}

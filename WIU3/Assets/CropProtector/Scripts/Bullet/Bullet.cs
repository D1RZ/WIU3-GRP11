using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] AudioClip DamageSound;
    [SerializeField] private float activeTime;

    private float instantiatedTime;

    private Vector2 instantiatedPosition;

    private void Start()
    {
        instantiatedTime = Time.time;
        instantiatedPosition = gameObject.transform.position;
    }

    private void Update()
    {
        if (Time.time > instantiatedTime + activeTime)
           Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hitbox")
        {
            if (collision.gameObject.transform.parent.gameObject.tag == "Locust")
            {
                Locust locust = collision.gameObject.transform.parent.gameObject.GetComponent<Locust>();

                if(PlayerController.instance.currentEquippedGun == PlayerController.Gun.Pistol || PlayerController.instance.currentEquippedGun == PlayerController.Gun.AR)
                   locust.Health -= 25;
                else if(PlayerController.instance.currentEquippedGun == PlayerController.Gun.Shotgun)
                {
                   if(Vector2.Distance(transform.position,instantiatedPosition) < 1.8f)
                   {
                      locust.Health -= 25;
                   }
                   else
                   {
                      locust.Health -= 2;
                   }
                }

                CropSoundManager.instance.PlaySoundFXClip(DamageSound,transform,AudioSettingsManager.instance.GetSFX());
                Destroy(gameObject);
            }
        }
    }
}

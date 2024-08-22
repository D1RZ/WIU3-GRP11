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

                if(PlayerController.instance.currentEquippedGun == PlayerController.Gun.Pistol)
                   locust.Health -= 25;
                else if(PlayerController.instance.currentEquippedGun == PlayerController.Gun.Shotgun)
                {
                    if (Vector2.Distance(gameObject.transform.position, instantiatedPosition) < 0.5f)
                        locust.Health -= 50;
                    else if (Vector2.Distance(gameObject.transform.position, instantiatedPosition) < 1.5f)
                        locust.Health -= 16.67f;
                    else if (Vector2.Distance(gameObject.transform.position, instantiatedPosition) < 2f)
                        locust.Health -= 8.3f;
                    else if (Vector2.Distance(gameObject.transform.position, instantiatedPosition) < 3f)
                        locust.Health -= 5f;
                    else
                        locust.Health -= 1;
                }
                else if(PlayerController.instance.currentEquippedGun == PlayerController.Gun.AR)
                    locust.Health -= 35;

                locust.currentState = Locust.State.Hit;

                CropSoundManager.instance.PlaySoundFXClip(DamageSound,transform,AudioSettingsManager.instance.GetSFX());
                Destroy(gameObject);
            }
            else if(collision.gameObject.transform.parent.gameObject.tag == "Mosquito")
            {
                Mosquito mosquito = collision.gameObject.transform.parent.gameObject.GetComponent<Mosquito>();

                if (PlayerController.instance.currentEquippedGun == PlayerController.Gun.Pistol)
                    mosquito.Health -= 25;
                else if (PlayerController.instance.currentEquippedGun == PlayerController.Gun.Shotgun)
                {
                    if (Vector2.Distance(gameObject.transform.position, instantiatedPosition) < 0.5f)
                        mosquito.Health -= 50;
                    else if (Vector2.Distance(gameObject.transform.position, instantiatedPosition) < 1.5f)
                        mosquito.Health -= 16.67f;
                    else if (Vector2.Distance(gameObject.transform.position, instantiatedPosition) < 2f)
                        mosquito.Health -= 8.3f;
                    else if (Vector2.Distance(gameObject.transform.position, instantiatedPosition) < 3f)
                        mosquito.Health -= 5f;
                    else
                        mosquito.Health -= 1;
                }
                else if (PlayerController.instance.currentEquippedGun == PlayerController.Gun.AR)
                    mosquito.Health -= 35;

                mosquito.currentState = Mosquito.State.Hit;

                CropSoundManager.instance.PlaySoundFXClip(DamageSound, transform, AudioSettingsManager.instance.GetSFX());
                Destroy(gameObject);
            }
        }
    }
}

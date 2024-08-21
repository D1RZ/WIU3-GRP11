using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acid : MonoBehaviour
{
    [SerializeField] AudioClip PlayerHurtSound;
    [SerializeField] private float activeTime;

    private float instantiatedTime;

    private PlayerController playerController;

    private SpriteRenderer playerSprite;

    private void Start()
    {
        instantiatedTime = Time.time;
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
            if(collision.gameObject.transform.parent.gameObject.layer == 6)
            {
                playerController = collision.gameObject.transform.parent.gameObject.GetComponent<PlayerController>();

                playerController.playerData.health -= 20;

                playerSprite = playerController.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();

                playerSprite.color = Color.red;

                CropSoundManager.instance.PlaySoundFXClip(PlayerHurtSound,playerController.gameObject.transform,AudioSettingsManager.instance.GetSFX());
                Destroy(gameObject);
            }
        }
    }
}

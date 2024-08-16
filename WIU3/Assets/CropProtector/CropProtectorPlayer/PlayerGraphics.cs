using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGraphics : MonoBehaviour
{

    private SpriteRenderer PlayerSprite;

    private float _timeElapsed;

    [SerializeField] private float DmgFlashTime;

    private void Start()
    {
        PlayerSprite = gameObject.GetComponent<SpriteRenderer>();
        _timeElapsed = 0;
    }

    private void Update()
    {
        if(PlayerSprite.color != new Color(1,1,1))
        {
            _timeElapsed += Time.deltaTime;
        }

        if(_timeElapsed >= DmgFlashTime)
        {
            PlayerSprite.color = Color.white;
            _timeElapsed = 0;
        }
    }
}

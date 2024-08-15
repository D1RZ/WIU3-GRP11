using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArcadeSceneLoader : MonoBehaviour
{
    [SerializeField] string SceneName;
    public void OnTriggerEnter2D(Collider2D other)
    {
        // Task 2c - Load the scene using SceneManager.LoadScene()
        SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
    }
    //private SceneLoader _sceneLoader;
    //[SerializeField] private new List<BoxCollider2D> _ArcadeEntry = new List<BoxCollider2D>();
    //[SerializeField] private string[] SceneName;
    //bool PlayerPressedE, PlayerEntered;

    //private void Start()
    //{
    //    _sceneLoader = new SceneLoader();
    //    PlayerPressedE = false;
    //    PlayerEntered = false;
    //}
    //private void Update()
    //{
    //    PlayerPressedE = Input.GetKeyDown(KeyCode.E);
    //}
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    int count = 0;
    //    foreach(var arcade  in _ArcadeEntry)
    //    {

    //        if(arcade.gameObject == collision.gameObject)
    //        {
    //            _sceneLoader.LoadScene(SceneName.GetValue(count).ToString());
    //        }
    //        else
    //        {
    //            PlayerEntered = false;
    //            count++;
    //        }
    //    }
    //}
}
//if (PlayerPressedE)
//{
//    _sceneLoader.LoadScene(SceneName.GetValue(count).ToString());
//}
//else
//    PlayerEntered = true;
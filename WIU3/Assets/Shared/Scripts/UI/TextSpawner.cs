using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextSpawner : MonoBehaviour
{
    public int amountToSpawn = 30;

    private void Start()
    {
        for(int i = 0; i < amountToSpawn; i++)
        {
            GameObject textObj = new GameObject($"Item {i}");
            textObj.transform.parent = transform;
            textObj.AddComponent<TextMeshProUGUI>();
            textObj.GetComponent<TextMeshProUGUI>().text = $"Item {i}";
            textObj.GetComponent<TextMeshProUGUI>().fontSize = 20;
        }
    }
}

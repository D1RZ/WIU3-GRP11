using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RecyclableData : ScriptableObject
{
    public enum RecyclableType
    {
        TRASH,
        PLASTIC,
        PAPER,
        METAL
    }
    public string recyclableName;
    public Sprite Image;
    public RecyclableType type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "SO/PlayerData")]

public class PlayerData : ScriptableObject
{
    public List<MedalData> Medal;

    public float movementSpeed;
}

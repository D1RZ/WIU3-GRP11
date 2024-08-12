using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MedalData", menuName = "SO/MenuData")]

public class MedalData : ScriptableObject
{
    [SerializeField] private int id; // differentiates the different medals obtainable
    [SerializeField] private int MedalsObtained; // keeps track of the amount of medals obtained by player for this particular medal
}

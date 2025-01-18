using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewWave", menuName = "ScriptableObjects/Wave")]
public class Wave : ScriptableObject
{
    public List<WavePart> waveParts; // List of enemy types and counts
    public float interval = 1; // delay interfal for the enemies
}
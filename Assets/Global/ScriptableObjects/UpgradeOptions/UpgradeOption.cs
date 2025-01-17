using System;
using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "UpgradeOption", menuName = "ScriptableObjects/UpgradeOption")]
public class UpgradeOption : ScriptableObject
{
    public Sprite image;
    public new string name;
    public string description;
    public int rarity; 
    public List<ActionParamPair> interactionActions;
}

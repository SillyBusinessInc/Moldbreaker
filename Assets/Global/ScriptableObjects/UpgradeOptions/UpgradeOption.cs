using System;
using UnityEngine;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
[CreateAssetMenu(fileName = "UpgradeOption", menuName = "ScriptableObjects/UpgradeOption")]
public class UpgradeOption : ScriptableObject
{
    public Sprite image;
    public new string name;
    public string description;
    public int rarity; 
    public string text1;
    public Sprite keyboardImage;
    public string text2;

    public List<ActionParamPair> interactionActions;
}

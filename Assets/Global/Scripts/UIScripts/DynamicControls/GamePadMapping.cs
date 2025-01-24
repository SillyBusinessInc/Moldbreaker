using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

[Serializable]
public class GamePadMapping
{
    public GamepadButton controlPath;
    public TMP_SpriteAsset icon;
    public Sprite sprite;
    public int index;
}
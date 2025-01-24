using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public abstract class InputMapping<T> where T : Enum
{
    public T controlPath;
    public TMP_SpriteAsset icon;
    public Sprite sprite;
    public int index;
}

[Serializable]
public class KeyboardMapping : InputMapping<Key> { }

[Serializable]
public class GamePadMapping : InputMapping<GamepadButton> { }
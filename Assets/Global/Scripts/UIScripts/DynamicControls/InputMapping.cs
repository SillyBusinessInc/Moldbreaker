using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;


public interface IInputMapping
{
    public bool IsCorrectInput(string value);
    public IconPathResult GetIconPathResult();
}
public abstract class InputMapping<T> : IInputMapping where T : Enum
{
    public T controlPath;
    public TMP_SpriteAsset icon;
    public Sprite sprite;
    public int index;
    
    public bool IsCorrectInput(string value)
    {
        var parsedEnum = Enum.Parse(typeof(T), value, true);
        return parsedEnum.Equals(controlPath);
    }
    
    public IconPathResult GetIconPathResult()
    {
        return new IconPathResult
        {
            icon = icon,
            sprite = sprite,
            index = index
        };
    }
}

[Serializable]
public class KeyboardMapping : InputMapping<Key> { }

[Serializable]
public class GamePadMapping : InputMapping<GamepadButton> { }
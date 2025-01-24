using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem;
using System;

[CreateAssetMenu(fileName = "ControlIconMapping", menuName = "Input System/Control Icon Mapping")]
public class ControlIconMapping : ScriptableObject
{
    [Header("Xbox Controller Mappings")]
    public List<GamePadMapping> xBoxMappings = new();

    [Header("Keyboard Mappings")]
    public List<KeyboardMapping> keyboardMappings = new();

    [Header("PlayStation Controller Mappings")]
    public List<GamePadMapping> playStationMappings = new();

    // return the icon spritesheet and index for the control path
    public IconPathResult GetIcon(TDeviceType deviceType, string controlPath)
    {
        IInputMapping[] gamepadMappings = null;
        
        controlPath = controlPath.Trim().ToLower();
        
        // We and fix some of the outliers, like shift, leftShoulder, leftTrigger
        // if it's not one of the 3, it will just stay the same
        controlPath = controlPath switch
        {
            "shift" => Key.LeftShift.ToString().ToLower(),
            "leftshoulder" or "left shoulder" or "l1" or "lb" => GamepadButton.LeftShoulder.ToString().ToLower(),
            "lefttrigger" or "left trigger" or "l2" or "lt" => GamepadButton.LeftTrigger.ToString().ToLower(),
            "rightshoulder" or "right shoulder" or "r1" or "rb" => GamepadButton.RightShoulder.ToString().ToLower(),
            "righttrigger" or "right trigger" or "r2" or "rt" => GamepadButton.RightTrigger.ToString().ToLower(),
            _ => controlPath
        };
        
        Debug.Log($"Control Device: {deviceType} & Path: {controlPath}");
        gamepadMappings = deviceType switch
        {
            TDeviceType.Keyboard => keyboardMappings.ToArray(),
            TDeviceType.XboxController => xBoxMappings.ToArray(),
            TDeviceType.PlayStationController => playStationMappings.ToArray(),
            _ => gamepadMappings
        };

        foreach (var mapping in gamepadMappings)
        {
            if (mapping.IsCorrectInput(controlPath))
                return mapping.GetIconPathResult();
        }
        return null;
    }
}
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
        List<GamePadMapping> gamepadMappings = null;
        
        switch (deviceType)
        {
            case TDeviceType.Keyboard:
                foreach (var mapping in keyboardMappings)
                {
                    controlPath = controlPath.Trim().ToLower() switch
                    {
                        "shift" => Key.LeftShift.ToString().ToLower(),
                        _ => controlPath
                    };

                    if (mapping.controlPath.ToString().ToLower() != controlPath.ToLower()) continue;

                    return new IconPathResult
                    {
                        icon = mapping.icon,
                        sprite = mapping.sprite,
                        index = mapping.index
                    };
                }
                break;

            case TDeviceType.XboxController:
                gamepadMappings = xBoxMappings;
                break;

            case TDeviceType.PlayStationController:
                gamepadMappings = playStationMappings;
                break;
        }
        
        if (gamepadMappings == null) return null;

        controlPath = controlPath.Trim().ToLower() switch
        {
            "leftshoulder" or "left shoulder" or "l1" or "lb" => "leftshoulder",
            "lefttrigger" or "left trigger" or "l2" or "lt" => "lefttrigger",
            _ => controlPath
        };
        
        foreach (var mapping in gamepadMappings)
        {
            int controlValue = GetGamepadButtonValue(controlPath.Trim()) ?? -1;
            int mappingValue = GetGamepadButtonValue(mapping.controlPath.ToString().Trim()) ?? -1;
            
            // Debug.Log($"{controlValue} = {mappingValue} - {mapping.controlPath.ToString().Trim()}");
            
            if (controlValue != mappingValue) continue;

            return new IconPathResult
            { 
                icon = mapping.icon,
                sprite = mapping.sprite,
                index = mapping.index
            };
        }
        
        return null;
    }

    private int? GetGamepadButtonValue(string controlPath)
    {
        if (Enum.TryParse<GamepadButton>(controlPath, true, out var result))
            return (int)result;
        
        return null;
    }
}
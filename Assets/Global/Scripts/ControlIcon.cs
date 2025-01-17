using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem;
using TMPro;

[CreateAssetMenu(fileName = "ControlIconMapping", menuName = "Input System/Control Icon Mapping")]
public class ControlIconMapping : ScriptableObject
{
    public class IconPathResult
    {
        public TMP_SpriteAsset icon;
        public int index;
    }

    // Enum for device types
    public enum TDeviceType
    {
        Keyboard,
        XboxController,
        PlayStationController,
        GenericGamepad
    }

    [System.Serializable]
    public class KeyboardMapping
    {
        public Key keyboardMapping;
        public TMP_SpriteAsset icon;
        public int index;
    }


    [System.Serializable]
    public class GamePadMapping
    {
        public GamepadButton controlPath;
        public TMP_SpriteAsset icon;
        public int index;
    }

    [Header("Control Icon Mappings")]

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
                    if (mapping.keyboardMapping.ToString().ToLower() == controlPath.ToLower())
                    {
                        return new IconPathResult
                        {
                            icon = mapping.icon,
                            index = mapping.index
                        };
                    }
                }
                break;

            case TDeviceType.XboxController:
                gamepadMappings = xBoxMappings;
                break;

            case TDeviceType.PlayStationController:
                gamepadMappings = playStationMappings;
                break; 
        }

        if (gamepadMappings != null)
        {
            foreach (var mapping in gamepadMappings)
            {
                int controlValue = GetGamepadButtonValue(controlPath) ?? -1;
                int mappingValue = GetGamepadButtonValue(mapping.controlPath.ToString()) ?? -1;

                if (controlValue == mappingValue)
                {
                    return new IconPathResult
                    {
                        icon = mapping.icon,
                        index = mapping.index
                    };
                }
            }
        }

        return null;  
    }

    private int? GetGamepadButtonValue(string controlPath)
    {
        // Get int value for comparison
        if (System.Enum.TryParse<GamepadButton>(controlPath, true, out var result))
        {
            Debug.Log($"Parsed control path: {result}");
            return (int)result;
        }

        return null;
    }

}
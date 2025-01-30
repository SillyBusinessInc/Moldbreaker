using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.XInput;

public class DevicesManager : MonoBehaviour
{
    string lastDeviceTypeUsed = "";
    TDeviceType lastKnownGamePadType; 

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    { 
        InputUser.onChange += OnChange;
    } 
    
    private void OnChange(InputUser _, InputUserChange change, InputDevice device)
    {
        if (change != InputUserChange.ControlSchemeChanged) return;
        GlobalReference.AttemptInvoke(Events.DEVICE_CHANGED);

        switch (device)
        {
            case DualShockGamepad:
                lastDeviceTypeUsed = "DualShockGamepad";
                lastKnownGamePadType = TDeviceType.PlayStationController;
                break;
            case XInputController:
                lastDeviceTypeUsed = "XInputController";
                lastKnownGamePadType = TDeviceType.XboxController;
                break;
            default:
                lastDeviceTypeUsed = "Unknown";
                break;
        }
    }
}

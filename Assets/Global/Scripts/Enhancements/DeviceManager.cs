using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.XInput;
using static ControlIconMapping;

public class DevicesManager : MonoBehaviour
{

    string lastDeviceTypeUsed = "";
    TDeviceType lastKnownGamePadType; 
 

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Test(InputUser _, InputUserChange change, InputDevice device)
    {
          if (change == InputUserChange.ControlSchemeChanged) {  
            GlobalReference.AttemptInvoke(Events.DEVICE_CHANGED);
        
            if (device is DualShockGamepad)
            {
                lastDeviceTypeUsed = "DualShockGamepad";
                lastKnownGamePadType = TDeviceType.PlayStationController;
            }
            else if (device is XInputController)
            {
                lastDeviceTypeUsed = "XInputController";
                lastKnownGamePadType = TDeviceType.XboxController;
            }
            else
            {   
                lastDeviceTypeUsed = "Unknown"; 
            }
            
    }
    }

    void Start()
    { 
        InputUser.onChange += Test;

    } 
}

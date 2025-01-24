using System;
using UnityEngine;

public class ScreenMode : MonoBehaviour
{
    void Start()
    {
        int mode = GlobalReference.Settings.Get<int>("screen_mode");
        int fpsMode = GlobalReference.Settings.Get<int>("framerate_mode");
        ChangeScreenMode(mode);
        ChangeFpsMode(fpsMode);

    }

    public void ChangeScreenMode(int mode)
    {
        Screen.fullScreenMode = mode switch
        {
            0 => FullScreenMode.FullScreenWindow,
            1 => FullScreenMode.Windowed,
            2 => FullScreenMode.ExclusiveFullScreen,
            _ => Screen.fullScreenMode
        };
    }
    
    public void ChangeFpsMode(int fpsMode)
    {
        Application.targetFrameRate = fpsMode switch
        {
            0 => 30,
            1 => 60,
            2 => 120,
            3 or _ => -1, // Unlimited
        };
    }
}

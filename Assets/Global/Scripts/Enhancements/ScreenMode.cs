using System;
using UnityEngine;
using System.Collections.Generic;


public class ScreenMode : MonoBehaviour
{
    List<(int width, int height)> resolutions = new List<(int width, int height)>
        {
            (1920, 1080),
            (1760, 990),
            (1680, 1050),
            (1600, 900),
            (1366, 768),
            (1280, 1024),
            (1280, 720),
            (1128, 634),
            (1024, 768), 
            (800, 600)
        };
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

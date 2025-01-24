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
        ChangeScreenMode(mode);
    }

    public void ChangeScreenMode(int mode)
    {
        switch (mode)
        {
            case 0: // Windowed
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;

            case 1: // Borderless Fullscreen
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;

            case 2: // Fullscreen
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;

            default:
                break;
        }
    }
}

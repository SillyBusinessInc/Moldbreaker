using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public static class SettingsHelper
{
    private static readonly Dictionary<int, (int width, int height)> 
        Resolutions = new Dictionary<int, (int , int)>
    { // IMPORTANT, don't change existing keys, only add new ones.
      // it does not matter if they become mixed up
        { 0, (4096, 2160) }, 
        { 1, (3840, 2160) },
        { 2, (2560, 1600) },
        { 3, (2560, 1440) },
        { 4, (2560, 1080) },
        { 5, (1920, 1440) },
        { 6, (1920, 1200) }, // ^  better than standard
        { 7, (1920, 1080) }, // standard
        { 8, (1760, 990) },  // v  worse than standard
        { 9, (1680, 1050) },
        { 10, (1600, 1200) },
        { 11, (1600, 1024) },
        { 12, (1600, 900) },
        { 13, (1440, 1080) },
        { 14, (1440, 900) },
        { 15, (1366, 768) },
        { 16, (1280,1024) },
        { 17, (1280, 960) },
        { 18, (1280, 800) },
        { 19, (1280, 720) },
        { 20, (1128, 634) },
        { 21, (1024, 768) },
        { 22, (800, 600) }
    };

    private static readonly Dictionary<int, (int fps, string optionName)>
        FpsOptions = new Dictionary<int, (int, string)>
        {
            // IMPORTANT, don't change existing keys, only add new ones.
            // it does not matter if they become mixed up
            { 0, (-1, "No Limit") },
            { 1, (30, "30 FPS") },
            { 2, (60, "60 FPS") },
            { 3, (75, "75 FPS") },
            { 4, (120, "120 FPS") },
            { 5, (144, "144 FPS") }
        };
    
    public static void ChangeScreenMode(int mode)
    {
        Screen.fullScreenMode = mode switch
        {
            0 => FullScreenMode.FullScreenWindow,
            1 => FullScreenMode.Windowed,
            2 => FullScreenMode.ExclusiveFullScreen,
            _ => Screen.fullScreenMode
        };
    }
    
    public static List<TMP_Dropdown.OptionData> GetFpsOptions()
    {
        return FpsOptions.Values.Select(
            x => new TMP_Dropdown.OptionData(x.optionName)
            ).ToList();
    }
    public static void ChangeFpsMode(int fpsMode)
    {
        if (FpsOptions.TryGetValue(fpsMode, out var fps))
            Application.targetFrameRate = fps.fps;
        else
            Debug.LogWarning($"FPS mode {fpsMode} not found");
    }

    public static List<TMP_Dropdown.OptionData> GetResolutionOptions()
    {
        return Resolutions.Values.Select(
            x => new TMP_Dropdown.OptionData($"{x.width} x {x.height}")
            ).ToList();
    }
    public static void ChangeResolutionMode(int mode)
    {
        if (Resolutions.TryGetValue(mode, out var resolution))
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
        else 
            Debug.LogWarning($"Resolution mode {mode} not found");
    }
}

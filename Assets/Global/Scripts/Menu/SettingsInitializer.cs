using UnityEngine;

public class SettingsInitializer : MonoBehaviour
{
    void Start()
    {
        var screenMode = GlobalReference.Settings.Get<int>("screen_mode");
        SettingsHelper.ChangeScreenMode(screenMode);
        
        var fpsMode = GlobalReference.Settings.Get<int>("framerate_mode");
        SettingsHelper.ChangeFpsMode(fpsMode);
        
        var resMode = GlobalReference.Settings.Get<int>("resolution");
        SettingsHelper.ChangeResolutionMode(resMode);
    }
}

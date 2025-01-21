using UnityEngine;

public class ScreenMode : MonoBehaviour
{
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

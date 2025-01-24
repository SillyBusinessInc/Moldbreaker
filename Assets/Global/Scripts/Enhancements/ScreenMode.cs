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
    public void ChangeFpsMode(int fpsMode)
    {
        switch (fpsMode)
        {
            case 0:
                Application.targetFrameRate = 30;
                break;

            case 1:
                Application.targetFrameRate = 60;
                break;

            case 2:
                Application.targetFrameRate = 120;
                break;
            case 3:
                Application.targetFrameRate = -1;
                break;

            default:
                break;
        }
    }
}

using UnityEngine;
using TMPro;


public class ScreenMode : MonoBehaviour
{
    int mode;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        mode = GlobalReference.Settings.Get<int>("screen_mode");
        ChangeScreenMode(mode);
    }

    public void ChangeScreenMode(int mode)
    {
        // Debug.Log("mode : " + mode);
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
        GlobalReference.Settings.Set("screen_mode", mode);
        // Debug.Log("ScreenMode : " + Screen.fullScreenMode);
    }
}

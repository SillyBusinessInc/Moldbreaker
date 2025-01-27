using UnityEngine;

public class ResolutionMode : MonoBehaviour
{
    private int width;
    private int height;
    void Start()
    {
        int mode = GlobalReference.Settings.Get<int>("resolution");
        ChangeResolutionMode(mode);
    }

    public void ChangeResolutionMode(int mode)
    {
        switch (mode)
        {
            case 0: 
                width = 1920;
                height = 1080;
                Screen.SetResolution(width, height, Screen.fullScreenMode); 
                break;

            case 1: 
                width = 1366;
                height = 768;
                Screen.SetResolution(width, height, Screen.fullScreenMode); 
                break;
            case 2: 
                width = 1280;
                height = 1024;
                Screen.SetResolution(width, height, Screen.fullScreenMode); 
                break;
            case 3: 
                width = 1024;
                height = 768;
                Screen.SetResolution(width, height, Screen.fullScreenMode); 
                break;
            default:
                break;
        }
    }
}

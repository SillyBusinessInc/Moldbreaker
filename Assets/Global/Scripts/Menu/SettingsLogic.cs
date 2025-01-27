using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsLogic : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    
    [Header("Imports")]
    [SerializeField] private TMP_Dropdown screenModeDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown frameRateDropdown;

    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider effectsVolume;
    [SerializeField] private Slider musicVolume;

    [SerializeField] private Button cancel;
    [SerializeField] private Button confirm;
    [SerializeField] private Button back;

    void Start()
    {
        GlobalReference.Settings.LoadAll();
        GlobalReference.AudioSettingSave.LoadAll();
        LoadFromLocal();
        UILogic.ShowCursor();
    }

    void Update() => UpdateButtonState();


    private void LoadFromLocal()
    {
        GlobalReference.Settings.IsLocked = true;
        GlobalReference.AudioSettingSave.IsLocked = true;

        screenModeDropdown.value = GlobalReference.Settings.Get<int>("screen_mode");
        resolutionDropdown.value = GlobalReference.Settings.Get<int>("resolution");
        //frameRateDropdown.value = GlobalReference.Settings.Get<int>("framerate_mode");
        
        masterVolume.value = GlobalReference.GetReference<AudioManager>().GetMasterVolume() / 8;
        effectsVolume.value = GlobalReference.GetReference<AudioManager>().GetSFXVolume() / 8;
        musicVolume.value = GlobalReference.GetReference<AudioManager>().GetMusicVolume() / 8;

        GlobalReference.GetReference<AudioManager>().LoadFromLocal();

        GlobalReference.Settings.IsLocked = false;
        GlobalReference.AudioSettingSave.IsLocked = false;
    }

    private void UpdateButtonState()
    {
        cancel.interactable = GlobalReference.Settings.IsDirty || GlobalReference.AudioSettingSave.IsDirty;
        confirm.interactable = GlobalReference.Settings.IsDirty || GlobalReference.AudioSettingSave.IsDirty;
        back.interactable = !(GlobalReference.Settings.IsDirty || GlobalReference.AudioSettingSave.IsDirty);
    }

    public void OnMasterVolumeChange(float value)
    {
        GlobalReference.GetReference<AudioManager>().UpdateMasterVolume(value * 8);
        GlobalReference.GetReference<AudioManager>().PlaySFX("AttackVOX2");
    }

    public void OnEffectsVolumeChange(float value)
    {
        GlobalReference.GetReference<AudioManager>().UpdateSFXVolume(value * 8);
        GlobalReference.GetReference<AudioManager>().PlaySFX("AttackVOX2");
    }

    public void OnMusicVolumeChange(float value)
    {
        GlobalReference.GetReference<AudioManager>().UpdateMusicVolume(value * 8);
    }

    public void OnBack()
    {
        GlobalReference.Settings.SaveAll();
        GlobalReference.AudioSettingSave.SaveAll();

        PauseLogic.ForceSelectDefault();
        
        SceneManager.UnloadSceneAsync("Settings");
    }

    public void OnSave()
    {
        GlobalReference.Settings.SaveAll();
        GlobalReference.AudioSettingSave.SaveAll();
    }

    public void OnCancel()
    {
        GlobalReference.Settings.LoadAll();
        GlobalReference.AudioSettingSave.LoadAll();
        LoadFromLocal();
    }

    public void OnScreenModeChange()
    {
        var mode = screenModeDropdown.value;
        Screen.fullScreenMode = mode switch
        {
            0 => FullScreenMode.FullScreenWindow,
            1 => FullScreenMode.Windowed,
            2 => FullScreenMode.ExclusiveFullScreen,
            _ => Screen.fullScreenMode
        };
        GlobalReference.Settings.Set("screen_mode", mode);
        Debug.Log($"FullScreen: {Screen.fullScreenMode}");
    }

    public void OnResolutionChange()
    {
        int mode = resolutionDropdown.value;

        GlobalReference.Settings.Set("resolution", mode);  
        string selectedOption = resolutionDropdown.options[mode].text;
        string[] resolution = selectedOption.Replace(" ", "").Split('x');
        int width = int.Parse(resolution[0].Trim()); 
        int height = int.Parse(resolution[1].Trim());
        Screen.SetResolution(width, height, Screen.fullScreenMode); 
    }

    public void OnFramerateChange()
    {
        var mode = frameRateDropdown.value;
        Application.targetFrameRate = mode switch
        {
            0 => 30,
            1 => 60,
            2 => 120,
            3 or _ => -1, // Unlimited
        };
        GlobalReference.Settings.Set("framerate_mode", mode);
    }
}
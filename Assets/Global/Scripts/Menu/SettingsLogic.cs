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

    [SerializeField] private Toggle speedrunMode;
    [SerializeField] private Toggle disableMouseLock;  
    
    [SerializeField] private Button cancel;
    [SerializeField] private Button confirm;
    [SerializeField] private Button back;

    void Start()
    {
        GlobalReference.Settings.LoadAll();
        GlobalReference.AudioSettingSave.LoadAll();
        LoadFromLocal();
        UILogic.SetCursor(true);
    }

    void Update() => UpdateButtonState();


    private void LoadFromLocal()
    {
        GlobalReference.Settings.IsLocked = true;
        GlobalReference.AudioSettingSave.IsLocked = true;

        resolutionDropdown.options = SettingsHelper.GetResolutionOptions();
        frameRateDropdown.options = SettingsHelper.GetFpsOptions();
        
        screenModeDropdown.value = GlobalReference.Settings.Get<int>("screen_mode");
        resolutionDropdown.value = GlobalReference.Settings.Get<int>("resolution");
        frameRateDropdown.value = GlobalReference.Settings.Get<int>("framerate_mode");
        
        masterVolume.value = GlobalReference.GetReference<AudioManager>().GetMasterVolume() / 8;
        effectsVolume.value = GlobalReference.GetReference<AudioManager>().GetSFXVolume() / 8;
        musicVolume.value = GlobalReference.GetReference<AudioManager>().GetMusicVolume() / 8;

        speedrunMode.isOn = GlobalReference.Settings.Get<bool>("speedrun_mode");
        disableMouseLock.isOn = GlobalReference.Settings.Get<bool>("disable_mouse_lock");
        
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
        GlobalReference.AttemptInvoke(Events.SPEEDRUN_MODE_TOGGLED);
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
        SettingsHelper.ChangeScreenMode(mode);
        GlobalReference.Settings.Set("screen_mode", mode);
    }

    public void OnResolutionChange()
    {
        var mode = resolutionDropdown.value;
        SettingsHelper.ChangeResolutionMode(mode);
        GlobalReference.Settings.Set("resolution", mode);  
    }

    public void OnSpeedRunModeChange() => GlobalReference.Settings.Set("speedrun_mode", speedrunMode.isOn);
    public void OnFramerateChange()
    {
        var mode = frameRateDropdown.value;
        SettingsHelper.ChangeFpsMode(mode);
        GlobalReference.Settings.Set("framerate_mode", mode);
    }
    
    public void OnDisableMouseLockChange() => GlobalReference.Settings.Set("disable_mouse_lock", disableMouseLock.isOn);
}
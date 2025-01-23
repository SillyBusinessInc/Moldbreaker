using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsLogic : MonoBehaviour
{
    [SerializeField] private Image fadeImage;

    [Header("Imports")]

    [SerializeField] private TMP_Dropdown screenModeDropdown;
    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider effectsVolume;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Toggle speedrunMode;

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

        masterVolume.value = GlobalReference.GetReference<AudioManager>().GetMasterVolume() / 8;
        effectsVolume.value = GlobalReference.GetReference<AudioManager>().GetSFXVolume() / 8;
        musicVolume.value = GlobalReference.GetReference<AudioManager>().GetMusicVolume() / 8;
        speedrunMode.isOn = GlobalReference.Settings.Get<bool>("speedrun_mode");

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
        GlobalReference.GetReference<AudioManager>().UpdateSFXVolume( value * 8);
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
        UILogic.FadeToScene("Menu", fadeImage, this);
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
        int mode = screenModeDropdown.value;
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
    }

    public void OnSpeedModeChange() => GlobalReference.Settings.Set("speedrun_mode", speedrunMode.isOn);
}
using NUnit.Framework;
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

        masterVolume.value = AudioManager.Instance.GetMasterVolume() / 8;
        effectsVolume.value = AudioManager.Instance.GetSFXVolume() / 8;
        musicVolume.value = AudioManager.Instance.GetMusicVolume() / 8;

        AudioManager.Instance.LoadFromLocal();

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
        AudioManager.Instance.UpdateMasterVolume(value * 8);
        AudioManager.Instance.PlaySFX("AttackVOX2");
    }

    public void OnEffectsVolumeChange(float value)
    {
        AudioManager.Instance.UpdateSFXVolume( value * 8);
        AudioManager.Instance.PlaySFX("AttackVOX2");
    }

    public void OnMusicVolumeChange(float value)
    {
        AudioManager.Instance.UpdateMusicVolume(value * 8);
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
}
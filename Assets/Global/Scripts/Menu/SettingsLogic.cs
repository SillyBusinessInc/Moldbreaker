using System;
using System.Linq;
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
        LoadFromSave();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update() => UpdateButtonState();


    private void LoadFromSave()
    {
        screenModeDropdown.value = GlobalReference.Settings.Get<int>("screen_mode");

        masterVolume.value = AudioManager.Instance.GetMasterVolume() /8;
        effectsVolume.value = AudioManager.Instance.GetSFXVolume() /8;
        musicVolume.value = AudioManager.Instance.GetMusicVolume() /8;

        GlobalReference.Settings.IsLocked = false;
    }

    private void UpdateButtonState()
    {
        cancel.interactable = GlobalReference.Settings.IsDirty;
        confirm.interactable = GlobalReference.Settings.IsDirty;
        back.interactable = !GlobalReference.Settings.IsDirty;
    }


    public void OnMasterVolumeChange(float value) => AudioManager.Instance.UpdateMasterVolume(value * 8);
    public void OnEffectsVolumeChange(float value) => AudioManager.Instance.UpdateSFXVolume(value* 8);
    public void OnMusicVolumeChange(float value) => AudioManager.Instance.UpdateMusicVolume(value* 8);


    public void OnBack()
    {
        GlobalReference.Settings.SaveAll();
        UILogic.FadeToScene("Menu", fadeImage, this);
    }
    public void OnSave()
    {
        GlobalReference.Settings.SaveAll();
    }
    public void OnCancel()
    {
        GlobalReference.Settings.LoadAll();
        LoadFromSave();
    }

    public void ChangeScreenMode()
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
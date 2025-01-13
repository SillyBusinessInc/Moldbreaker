using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsLogic : MonoBehaviour
{
    [SerializeField] private Image fadeImage;

    [Header("Imports")]
    [SerializeField] private TMP_Dropdown resolution;
    [SerializeField] private Toggle fullscreen;
    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider effectsVolume;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider brightness;

    [SerializeField] private Button cancel;
    [SerializeField] private Button confirm;
    [SerializeField] private Button back;

    void Start(){ 
        LoadFromSave();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    void Update() => UpdateButtonState();
    
    public static Resolution[] Resolutions => Screen.resolutions.Reverse().ToArray();

    private void LoadFromSave() {
        resolution.options = new();
        foreach (Resolution r in Resolutions) {
            resolution.options.Add(new($"{r.width} x {r.height}"));
        }

        GlobalReference.Settings.IsLocked = true;
        int resX = GlobalReference.Settings.Get<int>("resolution_width");
        int resY = GlobalReference.Settings.Get<int>("resolution_height");
        Resolution res = Resolutions.Where((x) => x.width == resX && x.height == resY).FirstOrDefault();
        resolution.value = -1;
        resolution.value = Array.IndexOf(Resolutions, res);
        fullscreen.isOn = GlobalReference.Settings.Get<bool>("fullscreen");

        masterVolume.value = AudioManager.Instance.GetMasterVolume();
        effectsVolume.value = AudioManager.Instance.GetSFXVolume();
        musicVolume.value = AudioManager.Instance.GetMusicVolume();

        brightness.value = GlobalReference.Settings.Get<float>("brightness");
        GlobalReference.Settings.IsLocked = false;
    }

    private void UpdateButtonState() {
        cancel.interactable = GlobalReference.Settings.IsDirty;
        confirm.interactable = GlobalReference.Settings.IsDirty;
        back.interactable = !GlobalReference.Settings.IsDirty;
    }
    
    #region button event methods
    public void OnResolutionChange(int value) {
        int resX = Resolutions[value].width;
        int resY = Resolutions[value].height;
        GlobalReference.Settings.Set("resolution_width", resX);
        GlobalReference.Settings.Set("resolution_height", resY);
    }
    public void OnFullscreenChange(bool value) => GlobalReference.Settings.Set("fullscreen", value);
    public void OnMasterVolumeChange(float value) => AudioManager.Instance.UpdateMusicVolume(value);
    public void OnEffectsVolumeChange(float value) => AudioManager.Instance.UpdateSFXVolume(value);
    public void OnMusicVolumeChange(float value) => AudioManager.Instance.UpdateMusicVolume(value);
    public void OnBrightnessChange(float value) => GlobalReference.Settings.Set("brightness", value);
    public void OnBack() {
        GlobalReference.Settings.SaveAll();
        UILogic.FadeToScene("Menu", fadeImage, this);
    }
    public void OnSave() {
        GlobalReference.Settings.SaveAll();
    }
    public void OnCancel() {
        GlobalReference.Settings.LoadAll();
        LoadFromSave();
    }
    #endregion
}
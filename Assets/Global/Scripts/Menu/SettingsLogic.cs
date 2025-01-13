using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsLogic : MonoBehaviour
{
    [SerializeField] private Image fadeImage;

    [Header("Imports")]

    [SerializeField] private TMP_Dropdown fullscreen;
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
        fullscreen.value = GlobalReference.Settings.Get<int>("fullscreen");

        masterVolume.value = AudioManager.Instance.GetMasterVolume();
        effectsVolume.value = AudioManager.Instance.GetSFXVolume();
        musicVolume.value = AudioManager.Instance.GetMusicVolume();

        GlobalReference.Settings.IsLocked = false;
    }

    private void UpdateButtonState()
    {
        cancel.interactable = GlobalReference.Settings.IsDirty;
        confirm.interactable = GlobalReference.Settings.IsDirty;
        back.interactable = !GlobalReference.Settings.IsDirty;
    }


    public void OnFullscreenChange(bool value) => GlobalReference.Settings.Set("fullscreen", value);
    public void OnMasterVolumeChange(float value) => AudioManager.Instance.UpdateMusicVolume(value);
    public void OnEffectsVolumeChange(float value) => AudioManager.Instance.UpdateSFXVolume(value);
    public void OnMusicVolumeChange(float value) => AudioManager.Instance.UpdateMusicVolume(value);
    public void OnBrightnessChange(float value) => GlobalReference.Settings.Set("brightness", value);


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
}
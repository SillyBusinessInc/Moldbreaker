using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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

    private AudioManager[] soundManagerList;
    private AudioManager soundManager;
    private AudioSource musicSource;
    private AudioSource sfxSource;


    void Start(){ 
        LoadFromSave();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void FindSoundManager() {
        soundManagerList = FindObjectsByType<AudioManager>(FindObjectsSortMode.None);
        soundManager = soundManagerList[0];
        musicSource = soundManager.musicSource;
        sfxSource = soundManager.SFXSource;
    }
    
    void Update() => UpdateButtonState();
    
    private void LoadFromSave() {
        GlobalReference.Settings.IsLocked = true;
        masterVolume.value = GlobalReference.Settings.Get<float>("master_volume");
        effectsVolume.value = GlobalReference.Settings.Get<float>("effects_volume");
        musicVolume.value = GlobalReference.Settings.Get<float>("music_volume");
        screenModeDropdown.value = GlobalReference.Settings.Get<int>("screen_mode");
        CalculateSPXSourceVolume(masterVolume.value, effectsVolume.value);
        CalculateMusicSourceVolume(masterVolume.value, musicVolume.value);
        GlobalReference.Settings.IsLocked = false;
    }

    private void UpdateButtonState() {
        cancel.interactable = GlobalReference.Settings.IsDirty;
        confirm.interactable = GlobalReference.Settings.IsDirty;
        back.interactable = !GlobalReference.Settings.IsDirty;
    }
    
    #region button event methods

    public void CalculateMusicSourceVolume(float masterVolume, float sourceVolume) {
        FindSoundManager();
        musicSource.volume = masterVolume * sourceVolume;
    }
    public void CalculateSPXSourceVolume(float masterVolume, float sourceVolume) {
        FindSoundManager();
        sfxSource.volume = masterVolume * sourceVolume;
    }


    public void OnFullscreenChange(bool value) => GlobalReference.Settings.Set("fullscreen", value);

    public void OnMasterVolumeChange(float value) {
        CalculateSPXSourceVolume(value, effectsVolume.value);
        CalculateMusicSourceVolume(value, musicVolume.value);
        GlobalReference.Settings.Set("master_volume", value);
    }
    
    public void OnEffectsVolumeChange(float value) {
        CalculateSPXSourceVolume(masterVolume.value, effectsVolume.value);
        GlobalReference.Settings.Set("effects_volume", value);
    }

    public void OnMusicVolumeChange(float value) {
        CalculateMusicSourceVolume(masterVolume.value, musicVolume.value);
        GlobalReference.Settings.Set("music_volume", value);
    } 

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


    public void ChangeScreenMode()
    {
        int mode = screenModeDropdown.value;
        switch (mode)
        {
            case 0: // Windowed
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;

            case 1: // Borderless Fullscreen
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
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

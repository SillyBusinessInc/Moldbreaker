using UnityEngine;
using System;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    [Header("Audio Sounds")]
    public Sound[] musicSounds, sfxSounds;

    [HideInInspector]
    public static AudioManager Instance;

    public AudioMixer audioMixer;
    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LoadAllSettings();
    }


    public void UpdateMusicVolume(float volume) => UpdateAudio("Music", volume);
    public void UpdateSFXVolume(float volume) => UpdateAudio("SFX", volume);
    public void UpdateMasterVolume(float volume) => UpdateAudio("Master", volume);
    public float GetMusicVolume() => GetVolume("Music");
    public float GetSFXVolume() => GetVolume("SFX");
    public float GetMasterVolume() => GetVolume("Master");
    public void PlayMusic(string name, Vector3? location = null) => PlaySound(name, false, true, location);
    public void PlayMusicOnRepeat(string name, Vector3? location = null) => PlaySound(name, true, true, location);
    public void PlaySFX(string name, Vector3? location = null) => PlaySound(name, false, false, location);
    public void PlaySFXOnRepeat(string name, Vector3? location = null) => PlaySound(name, true, false, location);


    public void StopMusicSound(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null || s.audioSource == null || s.clip == null) return;
        s.audioSource.Stop();
    }

    public void StopSFXSound(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null || s.audioSource == null || s.clip == null) return;
        s.audioSource.Stop();
    }


    private void UpdateAudio(string mixerString, float volume)
    {
        //values only from 0 to 8 will be accepted
        float tempVolume = volume;
        if (tempVolume > 8.0f)
        {
            tempVolume = 8.0f;
        }
        if (tempVolume < 0.0f)
        {
            tempVolume = 0.0f;
        }
        float convertedVolume = -80 + volume * 10;
        audioMixer.SetFloat(mixerString, convertedVolume);
        AudioSettingSave audioSettingSave = GlobalReference.AudioSettingSave;
        audioSettingSave.LoadAll();
        if (mixerString == "Master")
        {
            audioSettingSave.Set("Master", tempVolume);
        }
        if (mixerString == "SFX")
        {
            audioSettingSave.Set("SFX", tempVolume);
        }
        if (mixerString == "Music")
        {
            audioSettingSave.Set("Music", tempVolume);
        };
        audioSettingSave.SaveAll();
    }

    private float GetVolume(string param)
    {
        AudioSettingSave audioSettingSave = GlobalReference.AudioSettingSave;
        audioSettingSave.LoadAll();
        return audioSettingSave.Get<float>(param);
    }

    private void PlaySound(string name, bool repeat, bool music, Vector3? location = null)
    {

        Sound s = Array.Find(music ? musicSounds : sfxSounds, x => x.name == name);
        if (s == null || s.audioSource == null || s.clip == null) return;
        if (location == null)
        {
            s.audioSource.spatialBlend = 0.0f;
            s.audioSource.transform.position = Vector3.zero;
        }
        else
        {
            s.audioSource.transform.position = (Vector3)location;
            s.audioSource.spatialBlend = 1.0f;
        }
        s.audioSource.clip = s.clip;
        s.audioSource.loop = repeat;
        s.audioSource.Play();

    }

    private void LoadAllSettings()
    {
        AudioSettingSave audioSettingSave = GlobalReference.AudioSettingSave;
        audioSettingSave.LoadAll();
        UpdateMusicVolume(audioSettingSave.Get<float>("Music"));
        UpdateSFXVolume(audioSettingSave.Get<float>("SFX"));
        UpdateMasterVolume(audioSettingSave.Get<float>("Master"));
    }

}
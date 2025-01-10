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
    public void PlayMusic(string name) => PlaySound(name, false, true);
    public void PlayMusicOnRepeat(string name) => PlaySound(name, true, true);
    public void PlaySFX(string name) => PlaySound(name, false, false);
    public void PlaySFXOnRepeat(string name) => PlaySound(name, true, false);


    public void StopMusicSound(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null) return;
        s.audioSource.Stop();
    }

    public void StopSFXSound(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null) return;
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
            Debug.LogWarning($"SET: {tempVolume}");
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
        float volume;
        audioMixer.GetFloat(param, out volume);
        volume = -80 + volume * 10;
        return volume;
    }

    private void PlaySound(string name, bool repeat, bool music)
    {
        Sound s = Array.Find(music ? musicSounds : sfxSounds, x => x.name == name);
        if (s == null) return;
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
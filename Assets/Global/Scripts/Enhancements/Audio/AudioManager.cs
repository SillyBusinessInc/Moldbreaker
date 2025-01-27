using UnityEngine;
using System;
using UnityEngine.Audio;
public class AudioManager : Reference
{
    [Header("Audio Sounds")]
    public Sound[] musicSounds, sfxSounds;
    public AudioMixer audioMixer;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LoadFromLocal();
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
        var s = Array.Find(musicSounds, x => x.name == name);
        s?.audioSource?.Stop();
    }

    public void StopSFXSound(string name)
    {
        var s = Array.Find(sfxSounds, x => x.name == name);
        s?.audioSource?.Stop();
    }

    private void UpdateAudio(string mixerString, float volume)
    {
        var normalizedVolume = Mathf.Clamp01(volume / 8.0f);
        var dB = normalizedVolume > 0 ? Mathf.Lerp(-80, 0, Mathf.Log10(1 + 9 * normalizedVolume) / Mathf.Log10(10)) : -80;
        audioMixer.SetFloat(mixerString, dB);
        AudioSettingSave audioSettingSave = GlobalReference.AudioSettingSave;

        audioSettingSave.Set(mixerString, volume);
    }


    private float GetVolume(string param)
    {
        var audioSettingSave = GlobalReference.AudioSettingSave;
        return audioSettingSave.Get<float>(param);
    }

    private void PlaySound(string name, bool repeat, bool music, Vector3? location = null)
    {

        var s = Array.Find(music ? musicSounds : sfxSounds, x => x.name == name);
        if (s?.audioSource == null || s?.clip == null) return;
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

    public void LoadFromLocal()
    {
        var audioSettingSave = GlobalReference.AudioSettingSave;
        UpdateMusicVolume(audioSettingSave.Get<float>("Music"));
        UpdateSFXVolume(audioSettingSave.Get<float>("SFX"));
        UpdateMasterVolume(audioSettingSave.Get<float>("Master"));
    }
}
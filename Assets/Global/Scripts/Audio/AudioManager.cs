using UnityEngine;
using System;
public class AudioManager : MonoBehaviour
{
    [Header("Audio Sounds")]
    public Sound[] musicSounds, sfxSounds;

    [HideInInspector]
    public static AudioManager Instance;

    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null) return;
        s.audioSource.clip = s.clip;
        s.audioSource.Play();
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null) return;
        s.audioSource.clip = s.clip;
        s.audioSource.Play();
    }

    public void StopMusicSound(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        s.audioSource.Stop();
    }

    public void StopSFXSound(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        s.audioSource.Stop();
    }
}
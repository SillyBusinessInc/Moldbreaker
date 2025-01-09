using UnityEngine;
using System;
public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource musicSource, sfxSource;

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
        musicSource.clip = s.clip;
        musicSource.Play();
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null) return;
        sfxSource.clip = s.clip;
        sfxSource.Play();
    }

    public void StopMusicSource()
    {
        Instance.musicSource.Stop();
    }

    public void StopSFXSource()
    {
        Instance.sfxSource.Stop();
    }
}
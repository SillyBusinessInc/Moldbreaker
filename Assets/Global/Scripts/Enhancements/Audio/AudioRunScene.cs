using UnityEngine;

public class AudioRunScene : MonoBehaviour
{
    public string audioSongName;

    public bool loop = true;

    public void Start()
    {
        var am = GlobalReference.GetReference<AudioManager>();
        if (am == null) return;

        if (loop) am.PlayMusicOnRepeat(audioSongName);
        else am.PlayMusic(audioSongName);
    }

    void OnDestroy()
    {
        StopMusic();
    }

    public void StopMusic()
    {
        var am = GlobalReference.GetReference<AudioManager>();
        if (am == null) return;

        am.StopMusicSound(audioSongName);
    }
}
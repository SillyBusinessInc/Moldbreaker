using UnityEngine;

public class AudioRunScene : MonoBehaviour
{
    public string audioSongName;

    public bool loop = true;

    public void Start()
    {
        if (loop) GlobalReference.GetReference<AudioManager>().PlayMusicOnRepeat(audioSongName);
        else GlobalReference.GetReference<AudioManager>().PlayMusic(audioSongName);
    }

    void OnDestroy()
    {
        AudioManager am = GlobalReference.GetReference<AudioManager>();
        if (am != null) am.StopMusicSound(audioSongName);
    }
}
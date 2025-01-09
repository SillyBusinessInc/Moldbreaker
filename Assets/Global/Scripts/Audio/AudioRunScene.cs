using UnityEngine;


public class AudioRunScene : MonoBehaviour
{
    public string audioSongName;

    public void Start()
    {
        AudioManager.Instance.PlayMusic(audioSongName);
    }

    void OnDestroy()
    {
        AudioManager.Instance.StopMusicSound(audioSongName);
    }
}
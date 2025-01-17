using System.Threading;
using UnityEngine;


public class AudioRunScene : MonoBehaviour
{
    public string audioSongName;

    public bool loop = true;

    public void Start()
    {
        if(loop)
        {
            AudioManager.Instance.PlayMusicOnRepeat(audioSongName);
        }
        else
        {
            AudioManager.Instance.PlayMusic(audioSongName);
        }
    }

    void OnDestroy()
    {
        AudioManager.Instance.StopMusicSound(audioSongName);
    }
}
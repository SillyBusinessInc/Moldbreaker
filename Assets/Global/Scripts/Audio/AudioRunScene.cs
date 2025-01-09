using UnityEngine;


public class AudioRunScene : MonoBehaviour
{
    public string audioClipName;

    public void Start()
    {
        AudioManager.Instance.PlayMusic(audioClipName);
    }

    void OnDestroy()
    {
        AudioManager.Instance.StopMusicSource();
    }
}
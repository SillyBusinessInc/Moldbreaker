using UnityEngine;


public class AudioRunScene : MonoBehaviour
{
    public AudioClip audioClip;
    public void Start()
    {
        GlobalReference.GetReference<AudioManager>().PlaySFXOnRepeat(audioClip);
    }

    void OnDestroy()
    {
        GlobalReference.GetReference<AudioManager>().StopSFXLoop();
    }
}
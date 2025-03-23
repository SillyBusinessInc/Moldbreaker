using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class EntranceCutSceneLogic : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UILogic.SetCursor(true);
        videoPlayer.loopPointReached += VideoEnded;
    }

    void VideoEnded(VideoPlayer vp) {
        SceneManager.LoadScene("Loading");
    }

    public void OnExit() 
    {
        GlobalReference.GetReference<AudioManager>().PlaySFX("Button");
        VideoEnded(videoPlayer);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class CutSceneLogic : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Image fadeImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        videoPlayer.loopPointReached += VideoEnded;
    }

    void VideoEnded(VideoPlayer vp) {
        PlayerPrefs.SetInt("CutScenePlayed", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Loading");
    }

    public void OnExit() 
    {
        GlobalReference.GetReference<AudioManager>().PlaySFX("Button");
        UILogic.FadeToScene("Loading", fadeImage, this);
    }
}

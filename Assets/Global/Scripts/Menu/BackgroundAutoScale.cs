using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(AspectRatioFitter))]
public class BackgroundAutoScale : MonoBehaviour
{
    // it is very possible that these are null. so check for it
    private Image backgroundImage;
    private RawImage backgroundRawImage;
    private VideoPlayer backgroundVideoPlayer;
    
    private RectTransform rectTransform;
    private AspectRatioFitter ratioFitter;
    
    void Start()
    {
        backgroundImage = GetComponent<Image>();
        backgroundRawImage = GetComponent<RawImage>();
        backgroundVideoPlayer = GetComponent<VideoPlayer>();
        
        rectTransform = GetComponent<RectTransform>();
        ratioFitter = GetComponent<AspectRatioFitter>();

        ResetScale();
    }

    [ContextMenu("Reset Scale")]
    public void ResetScale()
    {
        // You can absolutely do this through the editor, I am just to lazy to do this for every background
        ratioFitter.aspectRatio = GetBackgroundAspect();
        ratioFitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
        rectTransform.sizeDelta = new Vector2(0, 0);
    }

    private float GetBackgroundAspect()
    {
        if (backgroundImage != null)
            return backgroundImage.sprite.rect.width / backgroundImage.sprite.rect.height;
        if(backgroundVideoPlayer != null)
            return backgroundVideoPlayer.width / (float)backgroundVideoPlayer.height;
        
        return backgroundRawImage.texture.width / (float)backgroundRawImage.texture.height;
    }
}

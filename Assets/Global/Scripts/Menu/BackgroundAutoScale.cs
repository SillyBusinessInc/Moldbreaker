using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(UnityEngine.UIElements.Image))]
[RequireComponent(typeof(AspectRatioFitter))]
public class BackgroundAutoScale : MonoBehaviour
{
    // You can absolutely do this through the editor, I am just to lazy to do this for every background
    private Image background;
    private RawImage backgroundRawImage;
    private RectTransform rectTransform;
    private AspectRatioFitter ratioFitter;
    
    void Start()
    {
        background = GetComponent<Image>();
        backgroundRawImage = GetComponent<RawImage>();
        rectTransform = GetComponent<RectTransform>();
        ratioFitter = GetComponent<AspectRatioFitter>();

        ResetScale();
    }

    [ContextMenu("Reset Scale")]
    public void ResetScale()
    {
        if (background != null) {
            ratioFitter.aspectRatio = background.sprite.rect.width / background.sprite.rect.height;
        } else if (backgroundRawImage != null) {
            ratioFitter.aspectRatio = (float)backgroundRawImage.texture.width / backgroundRawImage.texture.height;
        } else {
            Debug.LogError("No Image or RawImage added");
        }
        
        ratioFitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
        rectTransform.sizeDelta = new Vector2(0, 0);
    }
}

using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Foundation/UI Panel Scaler", 2)]
[RequireComponent(typeof(Image))]
[ExecuteInEditMode]
[DisallowMultipleComponent]
/// <summary>
/// Adds automatic scaling options for panels
/// </summary>
public class UIPanelScaler : UIScaler
{
    [Header("Panel Options")]
    [SerializeField] private float scale = 1;
    [SerializeField] private PanelScalingReference reference;

    private Image image;
    private Image Image => image == null? image = GetComponent<Image>() : image;

    new void Update()
    {
        base.Update();
        if (reference == PanelScalingReference.SCREEN_WIDTH) Image.pixelsPerUnitMultiplier = scale / (Screen.width / 1920.0f);
        else Image.pixelsPerUnitMultiplier = scale / (Screen.height / 1080);
    }

    private enum PanelScalingReference {
        SCREEN_WIDTH,
        SCREEN_HEIGHT
    }
}

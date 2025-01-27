using UnityEngine;

[AddComponentMenu("Foundation/UI Scaler", 1)]
[ExecuteInEditMode]
[DisallowMultipleComponent]
/// <summary>
/// Adds automatic scaling options
/// </summary>
public class UIScaler : MonoBehaviour
{
    [Header("Scaling Options")]
    [SerializeField] private bool autoScale = true;
    [SerializeField] private float padding = 0;

    private RectTransform rt;
    private RectTransform RectTransform => rt == null ? rt = GetComponent<RectTransform>() : rt;

    // field is never assigned warning
    #pragma warning disable 649
    private DrivenRectTransformTracker m_Tracker;
    #pragma warning restore 649

    protected void Update()
    {
        m_Tracker.Clear();
        if (autoScale) {
            m_Tracker.Add(this, RectTransform, DrivenTransformProperties.SizeDelta | DrivenTransformProperties.AnchoredPosition);

            RectTransform.offsetMin = new(-padding, -padding);
            RectTransform.offsetMax = new(padding, padding);
        }
    }

    void OnDisable()
    {
        m_Tracker.Clear();
    }
}

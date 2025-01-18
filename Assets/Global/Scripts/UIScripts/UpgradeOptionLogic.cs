using TMPro;
using UnityEngine;

public class UpgradeOptionLogic : MonoBehaviour
{
    public UpgradeOption data;
    public UnityEngine.UI.Image image;
    [HideInInspector] public TMP_Text upgradeName;
    [HideInInspector] public TMP_Text description;
    public TMP_Text text1;
    public TMP_Text text2;
    public UnityEngine.UI.Image keyboardImage;
    [SerializeField] private GameObject PressKeyboard;
    

    void Start()
    {
        upgradeName = transform.GetChild(0).GetComponent<TMP_Text>();
        description = transform.GetChild(1).GetComponent<TMP_Text>();
        SetData();
    }

    public void SetData()
    {
        image.sprite = data.image;
        upgradeName.text = data.name;
        description.text = data.description ?? "Hmm yes, yeast of power. So powerful";
        text1.text = data.text1;
        text2.text = data.text2;
        keyboardImage.sprite = data.keyboardImage;
        RectTransform rectTransform = PressKeyboard.GetComponent<RectTransform>();

        // if (text2.text == "") {
        //     RectTransform changedRectTransform = PressKeyboard.GetComponent<RectTransform>();
        //     Vector3 transform = new Vector3 (rectTransform.anchoredPosition.x + 22, rectTransform.anchoredPosition.y);
        //     changedRectTransform.anchoredPosition = transform;
        // } else {
        //     RectTransform OriginalrectTransform = PressKeyboard.GetComponent<RectTransform>();
        //     OriginalrectTransform = rectTransform;
        // }
        if (text2.text == "") {
            RectTransform rt = text1.GetComponent<RectTransform>();
            rt.anchorMin = new(0f, 0);
            rt.anchorMax = new(0.325f, 1);
            rt.offsetMin = new(0f, 0f);
            rt.offsetMax = new(0f, 0f);
            rt = keyboardImage.GetComponent<RectTransform>();
            rt.anchorMin = new(0.39f, 0);
            rt.anchorMax = new(0.91f, 1);
            rt.offsetMin = new(0f, 0f);
            rt.offsetMax = new(0f, 0f);
        }

        text1.fontSize = description.fontSize;
        text2.fontSize = description.fontSize;
    }
}
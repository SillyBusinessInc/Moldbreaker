using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundRandomizer : MonoBehaviour
{
    [SerializeField] private List<Sprite> backgrounds;
    [SerializeField] private List<string> tips;
    private Image background;
    private Image overlay;
    private TMP_Text tip_title;
    private TMP_Text tip_text;

    private float lastTimeSwitched = 0;
    private int lastIndex;
    [SerializeField] private float interval = 3;

    void Start() => Switch();
    void Update() => Switch();

    private void Switch() {
        if (lastTimeSwitched == -1 || lastTimeSwitched + interval > Time.unscaledTime) return;
        if (background == null) background = transform.GetChild(0).GetComponent<Image>();
        if (overlay == null) overlay = transform.GetChild(1).GetComponent<Image>();
        if (tip_title == null) tip_title = transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>();
        if (tip_text == null) tip_text = transform.GetChild(2).GetChild(1).GetComponent<TMP_Text>();

        lastTimeSwitched = -1;

        int index = Random.Range(0, backgrounds.Count-1);
        if (index == lastIndex) index = backgrounds.Count-1;
        lastIndex = index;

        string tip = tips[Random.Range(0, tips.Count)];
        tip_title.text = tip.Split(';')[0];
        tip_text.text = tip.Split(';')[1];

        Sprite sprite = backgrounds[index];
        overlay.sprite = sprite;
        SetAlpha(0);
        StartCoroutine(Fade());
    }

    private IEnumerator Fade() {
        for (float i = 0; i <= 1.1f; i += Time.unscaledDeltaTime)
        {
            SetAlpha(i);
            yield return null;
        }
        PostFade();
    }

    private void PostFade() {
        lastTimeSwitched = Time.unscaledTime;
        background.sprite = overlay.sprite;
        SetAlpha(0);
    }

    private void SetAlpha(float alpha) {
        overlay.color = new Color(1, 1, 1, alpha);
    }
}

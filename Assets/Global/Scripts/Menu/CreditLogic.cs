using System;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;

public class CreditLogic : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private GameObject panel;
    [SerializeField] private Image secret;
    [SerializeField] private GameObject line;
    [SerializeField] private GameObject inverseLine;

    private RectTransform rt;
    private RectTransform refObj;

    private float spacingImage = 0.4f;
    private float spacingTitle = 0.1f;
    private float spacingLine = 0.1f;
    private float spacingTeamImage = 0.5f;
    private float spacingBrandImage = 0.5f;
    private float spacingPadding = 0.1f;

    private float timeSinceHover = -1;
    private float timeSinceStart = -1;

    private float totalHeight = 0;

    void Start()
    {
        rt = panel.GetComponent<RectTransform>();
        timeSinceStart = Time.time;

        Populate();

        refObj = rt.GetChild(3) as RectTransform;
    }

    public void Populate() 
    {
        TextAsset creditData = (TextAsset)Resources.Load("Credits");
        string txt = creditData.text;
        string[] entries = txt.Split("\n");

        float spacingTotal = spacingImage + spacingTitle + spacingTeamImage + spacingBrandImage + spacingPadding * 4;
        spacingTotal += spacingLine * entries.Length;

        totalHeight = spacingTotal * Screen.height;

        // rt.anchorMin = new(0.5f, 0.1f);
        // rt.anchorMax = new(0.9f, 0.9f);
        // rt.anchoredPosition = new();

        RectTransform image = rt.GetChild(0) as RectTransform;
        SetPosition(image, 0, spacingImage);
        RectTransform title = rt.GetChild(1) as RectTransform;
        SetPosition(title, spacingImage + spacingPadding, spacingTitle);
        RectTransform teamImage = rt.GetChild(2) as RectTransform;
        SetPosition(teamImage, spacingTotal - spacingTeamImage - spacingBrandImage, spacingTeamImage);
        RectTransform brandImage = rt.GetChild(3) as RectTransform;
        SetPosition(brandImage, spacingTotal - spacingBrandImage, spacingBrandImage);

        for (int i = 0; i < entries.Count(); i++) {
            string entry = entries[i];
            float position = spacingImage + spacingTitle + spacingPadding * 2 + spacingLine * i;

            GameObject objToInstantiate = line;
            if (entry.StartsWith("[inverse]")) {
                entry = entry.Remove(0, 9);
                objToInstantiate = inverseLine;
            }

            GameObject newLine = Instantiate(objToInstantiate, new Vector3(), Quaternion.identity);
            newLine.GetComponent<RectTransform>().SetParent(rt);
            newLine.GetComponent<TMP_Text>().text = entry;
            SetPosition(newLine.GetComponent<RectTransform>(), position, spacingLine);
        }
    }

    private void SetPosition(RectTransform obj, float position, float size) 
    {
        obj.anchorMin = new(0.0f, 1 - (position + size));
        obj.anchorMax = new(1.0f, 1 - position);
        obj.anchoredPosition = new();
        obj.offsetMin = new(0f, 0f);
        obj.offsetMax = new(0f, 0f);
    }

    void Update()
    {
        if (rt.anchorMin.y < refObj.anchorMin.y * -1) {
            rt.anchorMin = new(0.5f, 0.075f * (Time.time - timeSinceStart));
            rt.anchorMax = new(0.9f, 0.075f * (Time.time - timeSinceStart) + 1);
            // rt.anchoredPosition = new();
            rt.offsetMin = new(0f, 0f);
            rt.offsetMax = new(0f, 0f);
        }
        else {
            rt.anchorMin = new(0.5f, refObj.anchorMin.y * -1);
            rt.anchorMax = new(0.9f, refObj.anchorMin.y * -1 + 1);
            // rt.anchoredPosition = new();
            rt.offsetMin = new(0f, 0f);
            rt.offsetMax = new(0f, 0f);
        }

        if (timeSinceHover == -1) secret.color = new(secret.color.r, secret.color.g, secret.color.b, 0);
        else secret.color = new(secret.color.r, secret.color.g, secret.color.b, Mathf.Clamp((Time.time - timeSinceHover) / 100, 0, 20));
    }

    public void OnExit() 
    {
        UILogic.FadeToScene("Menu", fadeImage, this);
    }

    public void OnHoverEnter() 
    {
        timeSinceHover = Time.time;
    }

    public void OnHoverExit() 
    {
        timeSinceHover = -1f;
    }
}

using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreditLogic : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private GameObject panel;
    [SerializeField] private Image secret;
    [SerializeField] private GameObject line;
    [SerializeField] private GameObject inverseLine;

    private RectTransform teamImage;
    private RectTransform brandImage;

    private RectTransform rt;

    private float spacingImage = 0.4f;
    private float spacingTitle = 0.1f;
    private float spacingLine = 0.1f;
    private float spacingTeamImage = 0.5f;
    private float spacingBrandImage = 0.5f;
    private float spacingPadding = 0.1f;

    // estimated time: 3.511 minutes

    private float timeSinceHover = -1;
    private float timeSinceStart = -1;
    private float delay = 1.0f;

    private float totalHeight = 0;
    private string[] entries;
    private int next = 0;
    private bool exiting = false;

    void Start()
    {
        rt = panel.GetComponent<RectTransform>();
        timeSinceStart = Time.time;

        Populate();
    }

    public void Populate() 
    {
        TextAsset creditData = (TextAsset)Resources.Load("Credits");
        string txt = creditData.text;
        entries = txt.Split("\n");

        float spacingTotal = spacingImage + spacingTitle + spacingTeamImage + spacingBrandImage + spacingPadding * 9;
        spacingTotal += spacingLine * entries.Length;

        totalHeight = spacingTotal * Screen.height;

        rt.anchorMin = new(0.5f, 0.075f * (Time.time - timeSinceStart));
        rt.anchorMax = new(0.9f, 0.075f * (Time.time - timeSinceStart) + 1);
        rt.offsetMin = new(0f, 0f);
        rt.offsetMax = new(0f, 0f);

        RectTransform image = rt.GetChild(0) as RectTransform;
        SetPosition(image, spacingPadding * 3, spacingImage);
        RectTransform title = rt.GetChild(1) as RectTransform;
        SetPosition(title, spacingImage + spacingPadding * 3, spacingTitle);
        teamImage = rt.GetChild(2) as RectTransform;
        SetPosition(teamImage, spacingTotal - spacingTeamImage - spacingBrandImage, spacingTeamImage);
        brandImage = rt.GetChild(3) as RectTransform;
        SetPosition(brandImage, spacingTotal - spacingBrandImage, spacingBrandImage);

        for (int i = 0; i < 10; i++) AddNext();

        teamImage.SetAsLastSibling();
        brandImage.SetAsLastSibling();
    }

    private void RemoveFirst() 
    {
        if (rt.childCount == 0) return;

        RectTransform firstObj = rt.GetChild(0) as RectTransform;
        // Debug.Log($"{firstObj.anchorMin.y}, {-0.075f * (Time.time - timeSinceStart - delay) + 1}");
        if (firstObj.anchorMin.y > -0.075f * (Time.time - timeSinceStart - delay) + 1) {

            Destroy(firstObj.gameObject);
            AddNext();
        }
    }

    private void AddNext() 
    {
        string entry = entries[next];
        float position = spacingImage + spacingTitle + spacingPadding * 7 + spacingLine * next;

        GameObject objToInstantiate = line;
        if (entry.StartsWith("[inverse]")) {
            entry = entry.Remove(0, 9);
            objToInstantiate = inverseLine;
        }

        GameObject newLine = Instantiate(objToInstantiate, new Vector3(), Quaternion.identity);
        RectTransform newLineTransform = newLine.GetComponent<RectTransform>();
        newLineTransform.SetParent(rt);
        newLine.GetComponent<TMP_Text>().text = entry;
        SetPosition(newLineTransform, position, spacingLine);

        teamImage.SetAsLastSibling();
        brandImage.SetAsLastSibling();

        next++;
        if (next < entries.Count() && newLineTransform.anchorMin.y >= -0.075f * (Time.time - timeSinceStart - delay)) AddNext();
    }

    private void SetPosition(RectTransform obj, float position, float size) 
    {
        obj.anchorMin = new(0.0f, 1 - (position + size));
        obj.anchorMax = new(1.0f, 1 - position);
        obj.offsetMin = new(0f, 0f);
        obj.offsetMax = new(0f, 0f);
    }

    void Update()
    {
        RemoveFirst();
        if (Time.time < timeSinceStart + delay) return;

        if (rt.anchorMin.y < brandImage.anchorMin.y * -1) {
            rt.anchorMin = new(0.5f, 0.075f * (Time.time - timeSinceStart - delay));
            rt.anchorMax = new(0.9f, 0.075f * (Time.time - timeSinceStart - delay) + 1);
            rt.offsetMin = new(0f, 0f);
            rt.offsetMax = new(0f, 0f);
        }
        else {
            rt.anchorMin = new(0.5f, brandImage.anchorMin.y * -1);
            rt.anchorMax = new(0.9f, brandImage.anchorMin.y * -1 + 1);
            rt.offsetMin = new(0f, 0f);
            rt.offsetMax = new(0f, 0f);
            if (!exiting) StartCoroutine(ExitAfter());
        }

        if (timeSinceHover == -1) secret.color = new(secret.color.r, secret.color.g, secret.color.b, 0);
        else secret.color = new(secret.color.r, secret.color.g, secret.color.b, Mathf.Clamp((Time.time - timeSinceHover) / 100, 0, 20));
    }

    private IEnumerator ExitAfter() {
        exiting = true;
        yield return new WaitForSeconds(delay);
        AchievementManager.Grant("SILLY_BUSINESS");
        OnExit();
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

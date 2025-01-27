using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

public class FetchAnnouncements : MonoBehaviour
{
    [SerializeField] private GameObject newsPanel;
    [SerializeField] private TMPro.TextMeshProUGUI newsText;
    [SerializeField] private Image bannerImage;
    [SerializeField] private Sprite fallbackImage;
    [SerializeField] private Button openLinkButton;
    [SerializeField] private float displayTime = 5f;
    [SerializeField] private int fetchLimit = 3;
    [SerializeField] private string appID = "480";
    // New serialized fields for dot indicators
    [SerializeField] private GameObject dotPrefab; // Assign a prefab with an Image component
    [SerializeField] private Transform dotContainer; // Parent transform for the dots
    [SerializeField] private Color activeDotColor = Color.white;
    [SerializeField] private Color inactiveDotColor = new Color(1, 1, 1, 0.5f);
    [SerializeField] private Color progressColor = Color.white;
    private string newsUrl => $"https://api.steampowered.com/ISteamNews/GetNewsForApp/v2/?appid={appID}&count={fetchLimit}";
    private const string SteamClanImageRoot = "https://clan.fastly.steamstatic.com/images/";

    private List<NewsItem> newsItems = new List<NewsItem>();
    private Dictionary<string, Sprite> imageCache = new Dictionary<string, Sprite>();
    private List<DotIndicator> dotIndicators = new List<DotIndicator>();
    private int currentNewsIndex = 0;
    private Coroutine cycleNewsCoroutine;

    private class DotIndicator
    {
        public Image backgroundImage;
        public Image fillImage;

        public DotIndicator(Image background, Image fill)
        {
            backgroundImage = background;
            fillImage = fill;
        }
    }

    void Start()
    {
        newsPanel.SetActive(false);
        if (openLinkButton) openLinkButton.onClick.AddListener(OpenCurrentNewsLink);
        StartCoroutine(FetchSteamNews());
    }

    IEnumerator FetchSteamNews()
    {
        UnityWebRequest request = UnityWebRequest.Get(newsUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            ProcessNews(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Failed to fetch news: " + request.error);
        }
    }

    void ProcessNews(string json)
    {
        var newsData = JsonConvert.DeserializeObject<SteamNewsResponse>(json);
        if (newsData?.appnews?.newsitems != null)
        {
            newsItems = newsData.appnews.newsitems;
            CreateDotIndicators(); // Create dots after getting news items
            StartCoroutine(PrefetchImages(newsItems));
        }
    }
    IEnumerator PrefetchImages(List<NewsItem> items)
    {
        foreach (var item in items)
        {
            string imageUrl = ExtractFirstImageUrl(item.contents);
            if (!string.IsNullOrEmpty(imageUrl) && !imageCache.ContainsKey(imageUrl))
            {
                UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(imageUrl);
                yield return imageRequest.SendWebRequest();

                if (imageRequest.result == UnityWebRequest.Result.Success)
                {
                    Texture2D texture = ((DownloadHandlerTexture)imageRequest.downloadHandler).texture;
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    imageCache[imageUrl] = sprite;
                }
                else
                {
                    Debug.LogError("Failed to prefetch image: " + imageRequest.error);
                    imageCache[imageUrl] = fallbackImage;
                }
            }

            if (cycleNewsCoroutine != null)
            {
                StopCoroutine(cycleNewsCoroutine);
            }
            cycleNewsCoroutine = StartCoroutine(CycleNews());
        }
    }

    IEnumerator CycleNews()
    {
        newsPanel.SetActive(true);

        while (true)
        {
            // Display current news item and update dots
            DisplayNews(newsItems[currentNewsIndex]);
            UpdateDotIndicators();

            // Animate the fill over time
            float elapsedTime = 0f;
            DotIndicator currentDot = dotIndicators[currentNewsIndex];

            while (elapsedTime < displayTime)
            {
                elapsedTime += Time.deltaTime;
                float fillAmount = Mathf.Clamp01(elapsedTime / displayTime);
                currentDot.fillImage.fillAmount = fillAmount;
                yield return null;
            }

            // Move to next news item
            currentNewsIndex = (currentNewsIndex + 1) % newsItems.Count;
        }
    }

    void DisplayNews(NewsItem newsItem)
    {
        newsText.text = $"{newsItem.title}";

        string imageUrl = ExtractFirstImageUrl(newsItem.contents);
        if (!string.IsNullOrEmpty(imageUrl) && imageCache.ContainsKey(imageUrl))
        {
            bannerImage.sprite = imageCache[imageUrl];
        }
        else
        {
            bannerImage.sprite = fallbackImage;
        }
    }
    string ExtractFirstImageUrl(string contents)
    {
        // Regex to find [img] tags and extract the image URL
        Match match = Regex.Match(contents, @"\[img\](.+?)\[\/img\]");
        if (match.Success)
        {
            string imageUrl = match.Groups[1].Value;
            if (imageUrl.Contains("{STEAM_CLAN_IMAGE}"))
            {
                imageUrl = imageUrl.Replace("{STEAM_CLAN_IMAGE}", SteamClanImageRoot);
            }
            return imageUrl;
        }
        return null;
    }

    void OpenCurrentNewsLink()
    {
        if (newsItems.Count > 0)
        {
            string url = newsItems[currentNewsIndex].url;
            if (!string.IsNullOrEmpty(url))
            {
                Application.OpenURL(url);
            }
        }
    }
    void CreateDotIndicators()
    {
        foreach (Transform child in dotContainer)
        {
            Destroy(child.gameObject);
        }
        dotIndicators.Clear();

        for (int i = 0; i < newsItems.Count; i++)
        {
            GameObject dot = Instantiate(dotPrefab, dotContainer);

            // Get both the background and fill images
            Image backgroundImage = dot.GetComponent<Image>();
            Image fillImage = dot.transform.GetChild(0).GetComponent<Image>(); // Assumes first child is the fill image

            dotIndicators.Add(new DotIndicator(backgroundImage, fillImage));

            // Set initial colors
            backgroundImage.color = inactiveDotColor;
            fillImage.color = progressColor;
            fillImage.fillAmount = 0f; // Start with empty fill
        }

        if (dotIndicators.Count > 0)
        {
            dotIndicators[0].backgroundImage.color = activeDotColor;
        }
    }
    void UpdateDotIndicators()
    {
        for (int i = 0; i < dotIndicators.Count; i++)
        {
            dotIndicators[i].backgroundImage.color = (i == currentNewsIndex) ? activeDotColor : inactiveDotColor;
            if (i != currentNewsIndex)
            {
                dotIndicators[i].fillImage.fillAmount = 0f;
            }
        }
    }
}

[System.Serializable]
public class SteamNewsResponse
{
    public AppNews appnews;
}

[System.Serializable]
public class AppNews
{
    public List<NewsItem> newsitems;
}

[System.Serializable]
public class NewsItem
{
    public string title;
    public string contents;
    public string url;
}

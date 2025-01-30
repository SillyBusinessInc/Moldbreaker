using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AnnouncementsCache", menuName = "ScriptableObjects/Announcements Cache")]
public class AnnouncementsCache : ScriptableObject
{
    [Serializable]
    public class CachedImage
    {
        public string url;
        public Sprite sprite;
    }

    public string steamNewsJson;
    public string sideloadJson;
    public DateTime lastFetchTime;
    [SerializeField] public List<CachedImage> cachedImages = new List<CachedImage>();

    [SerializeField] private float cacheDurationHours = 0.1f;

    public bool NeedsRefresh()
    {
        // Check if we have any cached data
        if (string.IsNullOrEmpty(steamNewsJson) && string.IsNullOrEmpty(sideloadJson))
            return true;

        // Check if cache has expired
        TimeSpan timeSinceLastFetch = DateTime.Now - lastFetchTime;
        return timeSinceLastFetch.TotalHours >= cacheDurationHours;
    }

    public void SaveNewsData(string steamNews, string sideload)
    {
        if (steamNewsJson != null) steamNewsJson = steamNews;
        if (sideloadJson != null) sideloadJson = sideload;
        lastFetchTime = DateTime.Now;
    }

    public void SaveImage(string url, Sprite sprite)
    {
        // Remove existing cache entry if it exists
        cachedImages.RemoveAll(x => x.url == url);

        // Add new cache entry
        cachedImages.Add(new CachedImage { url = url, sprite = sprite });
    }

    public Sprite GetCachedImage(string url)
    {
        return cachedImages.Find(x => x.url == url)?.sprite;
    }

    public void ClearCache()
    {
        steamNewsJson = null;
        sideloadJson = null;
        cachedImages.Clear();
    }
}
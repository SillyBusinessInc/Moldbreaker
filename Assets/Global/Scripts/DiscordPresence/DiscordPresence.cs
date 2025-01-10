using System;
using UnityEngine;
using Discord;
using UnityEngine.SceneManagement;

public class DiscordPresence : MonoBehaviour
{
    private static DiscordPresence instance;

    private Discord.Discord discord;
    private ActivityManager activityManager;
    private string lastSceneName = "";
    private long sessionStartTime; // Persistent timestamp

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Persist this GameObject across scene loads
    }

    private void Start()
    {
        InitializeDiscord();
        sessionStartTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); 
        UpdatePresence();
        lastSceneName = GetCurrentSceneName(); 
    }

    private void InitializeDiscord()
    {
        const long applicationId = 1316852538561138738; 
        discord = new Discord.Discord(applicationId, (ulong)Discord.CreateFlags.Default);
        activityManager = discord.GetActivityManager();
    }

    public void UpdatePresence()
    {
        string currentScene = GetCurrentSceneName();
        string details;
        string largeImageKey = "game_icon"; // Default image key

        // Update presence details and image based on the current scene
        switch (currentScene)
        {
            case "Menu":
            case "Title":
            case "Loading":
                details = "Browsing the menus";
                largeImageKey = "game_icon"; // Default image key
                break;
                
            case "Death":
                details = "Game Over..";
                largeImageKey = "game_icon"; // Default image key
                break;

            case "ENTRANCE_1":
                details = "Chillin' in the hub!";
                largeImageKey = "entrance"; // Use an appropriate image key
                break;

            case "PARKOUR_1":
                details = "Level 1";
                largeImageKey = "parkour_1"; // Use an appropriate image key
                break;

            case "PARKOUR_2":
                details = "Level 2";
                largeImageKey = "parkour_2"; // Use an appropriate image key
                break;

            case "PARKOUR_3":
                details = "Level 3";
                largeImageKey = "parkour_3"; // Use an appropriate image key
                break;

            default: // Unknown state
                details = "Playing the game"; 
                break;
        }

        var activity = new Activity
        {
            Details = details,
            Timestamps =
            {
                Start = sessionStartTime, // Use persistent session start time
            },
            Assets =
            {
                LargeImage = largeImageKey,
                LargeText = "Moldbreaker: Rise of the Loaf"
            }
        };

        activityManager.UpdateActivity(activity, result =>
        {
            if (result == Discord.Result.Ok)
            {
                Debug.Log("Discord Rich Presence updated successfully.");
            }
            else
            {
                Debug.LogError($"Failed to update Discord Rich Presence: {result}");
            }
        });
    }

    private void Update()
    {
        discord.RunCallbacks();

        // Update presence if the scene changes
        string currentScene = GetCurrentSceneName();
        if (currentScene != lastSceneName)
        {
            lastSceneName = currentScene;
            UpdatePresence();
        }
    }

    private string GetCurrentSceneName()
    {
        // Loop through all loaded scenes and return the most specific one
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded && scene.name != "BaseScene" && scene.name != "DontDestroyOnLoad")
            {
                return scene.name;
            }
        }

        // Default to active scene if no specific match
        return SceneManager.GetActiveScene().name;
    }

    private void OnApplicationQuit()
    {
        discord.Dispose();
    }
}

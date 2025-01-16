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
    private bool isDiscordInitialized = false;
    private float retryCooldown = 5f; // Time till retry discord presence 
    private float nextRetryTime = 0f; 

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
        TryInitializeDiscord();
        if (isDiscordInitialized)
        {
            sessionStartTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            UpdatePresence();
            lastSceneName = GetCurrentSceneName();
        }
    }

    private void TryInitializeDiscord()
    {
        try
        {
            const long applicationId = 1316852538561138738;
            discord = new Discord.Discord(applicationId, (ulong)Discord.CreateFlags.NoRequireDiscord);
            activityManager = discord.GetActivityManager();
            isDiscordInitialized = true; // Successfully initialized Discord
            // Debug.Log("Discord initialized successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Discord could not be initialized: {ex.Message}");
            isDiscordInitialized = false; // Mark Discord as unavailable
        }
    }

    public void UpdatePresence()
    {
        if (!isDiscordInitialized)
        {
            Debug.LogWarning("Discord is not initialized. Skipping Rich Presence update.");
            return;
        }

        string currentScene = GetCurrentSceneName();
        string details;
        string largeImageKey = "game_icon"; 

        // Update presence details and image based on the current scene
        switch (currentScene)
        {
            case "Menu":
            case "Title":
            case "Settings":
            case "Loading":
                details = "Browsing the menus";
                largeImageKey = "game_icon"; 
                break;

            case "Death":
                details = "Failed to cleanse the world of mold..";
                largeImageKey = "game_over"; 
                break;

            case "Credits":
                details = "Watching the credits";
                largeImageKey = "credits"; 
                break;

            case "ENTRANCE_1":
                details = "Chillin' in the hub!";
                //details = "Loafin' around in the hub!";
                largeImageKey = "entrance"; 
                break;

            case "PARKOUR_1":
                details = "Level 1";
                largeImageKey = "parkour_1"; 
                break;

            case "PARKOUR_2":
                details = "Level 2";
                largeImageKey = "parkour_2"; 
                break;

            case "PARKOUR_3":
                details = "Level 3";
                largeImageKey = "parkour_3"; 
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
                // Debug.Log("Discord Rich Presence updated successfully.");
            }
            else
            {
                Debug.LogError($"Failed to update Discord Rich Presence: {result}");
            }
        });
    }

    private void Update()
    {
        if (isDiscordInitialized)
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
        else
        {
            // Retry initializing Discord 
            if (Time.unscaledTime >= nextRetryTime)
            {
                Debug.Log("Retrying Discord initialization...");
                TryInitializeDiscord();

                if (isDiscordInitialized)
                {
                    sessionStartTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    UpdatePresence(); 
                }

                nextRetryTime = Time.unscaledTime + retryCooldown; // Set next retry time
            }
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
        if (isDiscordInitialized)
        {
            discord.Dispose();
        }
    }
}

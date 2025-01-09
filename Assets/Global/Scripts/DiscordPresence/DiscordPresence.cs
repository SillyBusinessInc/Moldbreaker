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
        lastSceneName = SceneManager.GetActiveScene().name; 
    }

    private void InitializeDiscord()
    {
        const long applicationId = 1316852538561138738; 
        discord = new Discord.Discord(applicationId, (ulong)Discord.CreateFlags.Default);
        activityManager = discord.GetActivityManager();
    }

    public void UpdatePresence()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        string details;

        // Update presence details based on the current scene
        switch (currentScene)
        {
            case "Menu":
            case "Title":
            case "Loading":
                details = "Browsing the menus";
                break;

            case "BaseScene":
                details = "In a run!";
                break;

           case "Death":
                details = "Failed a run..";
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
                LargeImage = "game_icon",
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
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != lastSceneName)
        {
            lastSceneName = currentScene;
            UpdatePresence();
        }
    }

    private void OnApplicationQuit()
    {
        discord.Dispose();
    }
}
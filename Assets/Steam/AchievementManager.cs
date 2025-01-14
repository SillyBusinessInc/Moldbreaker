using Steamworks;
using UnityEngine;

public static class AchievementManager
{
    public static void Grant(string name) {
        if (SteamManager.Initialized) {
            SteamUserStats.SetAchievement(name);
            SteamUserStats.StoreStats();
        }
    }
}

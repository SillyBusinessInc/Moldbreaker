using System.Collections.Generic;
using System.Linq;
using Steamworks;
using UnityEngine;

public static class AchievementManager
{
    private static Dictionary<string, bool> progression = null;
    public static void Init() {
        List<string> list = new() {
            "LIFE_IS_MOLDY",
            "BEGONE_MOLD",
            "SKILL_ISSUE",
            "RISE_EVEN_HIGHER",
            "LEAP_OF_FAITH",
            "RISE_OF_THE_LOAF",
            "SILLY_BUSINESS",
            "CRUMB_COLLECTOR_1",
            "CRUMB_COLLECTOR_2",
            "CRUMB_COLLECTOR_3",
            "CALORIE_GAINER_1",
            "CALORIE_GAINER_2",
            "CALORIE_GAINER_3",
            "LOAF_COMPLETIONIST",
            "MOLDBREAKER",
            "PAN_CHAN"
        };
        
        progression = new();
        list.ForEach(x => {
            if (SteamUserStats.GetAchievement(x, out bool achieved)) progression.Add(x, achieved);
        });
    }

    public static void Grant(string name) {
        if (!SteamManager.Initialized) return;
        if (progression == null) Init();

        if (!SteamUserStats.GetAchievement(name, out bool achieved)) return;
        if (achieved) return;

        SteamUserStats.SetAchievement(name);
        SteamUserStats.StoreStats();

        progression[name] = true;

        CheckGameCompletion();
    }

    public static void CheckLevelsCompletion() {
        if (!SteamManager.Initialized) return;
        if (progression == null) Init();

        if (progression["LOAF_COMPLETIONIST"]) return;

        if (!progression["CRUMB_COLLECTOR_1"] ||
            !progression["CRUMB_COLLECTOR_2"] ||
            !progression["CRUMB_COLLECTOR_3"] ||
            !progression["CALORIE_GAINER_1"]  ||
            !progression["CALORIE_GAINER_2"]  ||
            !progression["CALORIE_GAINER_3"]
        ) return;

        Grant("LOAF_COMPLETIONIST");
    }

    public static void CheckGameCompletion() {
        if (!SteamManager.Initialized) return;
        if (progression == null) Init();

        if (progression["MOLDBREAKER"]) return;

        // progression.Keys.ToList().ForEach(x => Debug.Log($"{x}, {progression[x]}"));
        if (!progression["LIFE_IS_MOLDY"]       ||
            !progression["BEGONE_MOLD"]         ||
            !progression["SKILL_ISSUE"]         ||
            !progression["RISE_EVEN_HIGHER"]    ||
            !progression["LEAP_OF_FAITH"]       ||
            !progression["RISE_OF_THE_LOAF"]    ||
            !progression["SILLY_BUSINESS"]      ||
            !progression["CRUMB_COLLECTOR_1"]   ||
            !progression["CRUMB_COLLECTOR_2"]   ||
            !progression["CRUMB_COLLECTOR_3"]   ||
            !progression["CALORIE_GAINER_1"]    ||
            !progression["CALORIE_GAINER_2"]    ||
            !progression["CALORIE_GAINER_3"]    ||
            !progression["LOAF_COMPLETIONIST"]  ||
            !progression["PAN_CHAN"]
        ) return;

        Grant("MOLDBREAKER");
    }
}
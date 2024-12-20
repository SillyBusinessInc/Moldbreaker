
using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

/*
    !!IMPORTANT
    To add a new statistic, add a new field that is of type PermanentStatistic
    Then in the Setup method, instantiate it with the name of the stat and add 'this' as the second parameter to being able to save the stat
    In the Stats field you add the new stat

    If the field will be a string, float, bool or int you can just use Add(statname, defaultValue); in the Init() method

    Then, in PlayerStatistic.cs you add a new field, but instead of PermanentStatistic, CurrentStatistic will be the type
    In the Generate() method in PlayerStatistic.cs you need to instatiate. The 1st parameter is the baseValue and the 2nd parameter is the PermanemtStatistic
    To call the PermanentStatistic you call it like GlobalReference.PermanentPlayerStatistic.fieldname

    To get the crumbs, you can to call
    GlobalReference.PermanentPlayerStatistic.Get<int>("crumbs")
    adding crumbs -> GlobalReference.PermanentPlayerStatistic.ModifyCrumbs(amount);
    removing crumbs -> GlobalReference.PermanentPlayerStatistic.ModifyCrumbs(-amount);

    For the other statistics, you are not able to get the permanentStatistics directly. You need to instantiate the PlayerStatistic class. 
    Then you can call PlayerStatistic.GetValue() which will give you the value with all the added multipliers and modifiers including the permanent ones
*/

[Serializable]
public class PermanentPlayerStatistic : SecureSaveSystem
{
    public PermanentStatistic Speed;
    public PermanentStatistic JumpForce;
    public PermanentStatistic MaxHealth;
    public PermanentStatistic AttackSpeedMultiplier;
    public PermanentStatistic AttackDamageMultiplier;
    public PermanentStatistic DodgeCooldown;
    public PermanentStatistic DoubleJumpsCount;
    protected override string Prefix => "PermanentPlayerStatistic";
    private PermanentStatistic[] Stats;

    private void Setup() {
        Speed = new("speed", this);
        JumpForce = new("jumpForce", this);
        MaxHealth = new("maxHealth", this);
        AttackSpeedMultiplier = new("attackSpeedMultiplier", this);
        AttackDamageMultiplier = new("attackDamageMultiplier", this);
        DodgeCooldown = new("dodgeCooldown", this);
        DoubleJumpsCount = new("doubleJumpsCount", this);

        Stats = new[] {
            Speed, JumpForce, MaxHealth, AttackSpeedMultiplier, AttackDamageMultiplier, DodgeCooldown, DoubleJumpsCount
        };
    }

    public override void Init() {
        Setup();
        
        // these are just default values
        foreach (var stat in Stats) {
            Add(stat.Param, "");
        }
        Add("crumbs", 0);
    }

    public void Generate() {
        // putting the saved values in the multipliers and modifiers list
        foreach (var stat in Stats) {
            stat.DeserializeModifications(Get<string>(stat.Param));
        }
    }

    public void ModifyCrumbs(int amount) {
        var crumbs = Get<int>("crumbs") + amount;
        if (crumbs < 0) crumbs = 0;
        Set("crumbs", crumbs);
        SaveAll();
        GlobalReference.AttemptInvoke(Events.CRUMBS_CHANGED);
    }
}
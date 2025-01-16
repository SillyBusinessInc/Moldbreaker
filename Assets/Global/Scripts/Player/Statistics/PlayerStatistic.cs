
using System.Collections.Generic;
using UnityEngine;

// IF you want to make this a Scriptable ,
// - uncomment the `Create AssetMenu`
// - uncomment the `Scriptable Object`
// - comment `[System.Serializable]`
// - remove `the new()` IN THE `Player.cs`, Not this script.  It is a scriptable object, so you should create it like that and not created it with new() in the player script.

[System.Serializable]
//[CreateAssetMenu(fileName = "PlayerStatistic", menuName = "PlayerStatistic")]
public class PlayerStatistic
{
    private float health;
    public float Health { 
        get => health = Mathf.Min(health, MaxHealth.GetValue());
        set => health = value > 0 ? value : 0;
    }

    private float moldmeter;
    private float maxmoldmeter = 100f;

    public float Moldmeter {
        get => moldmeter = Mathf.Min(moldmeter, maxmoldmeter);
        set => moldmeter = value > 0 ? value : 0;
    }

    [HideInInspector]private int crumbs;
    [HideInInspector]public int Crumbs {
        get => crumbs;
        set => crumbs = value > 0 ? value : 0;
    }
    [HideInInspector]public List<string> Calories;
    [HideInInspector]public int CaloriesCount;
    [HideInInspector]public int CrumbsCount;
    [HideInInspector]public bool CollectedCrumb = false;
    [HideInInspector]public bool CollectedCalorie = false;
    [HideInInspector]public int caloriesCountExtra = 0;
    [HideInInspector]public List<string> CaloriesCollected;

    // this is for the current stats of the player
    public CurrentStatistic Speed = new(12f);
    public CurrentStatistic JumpForce = new(8f);
    public CurrentStatistic MaxHealth = new(100f);
    public CurrentStatistic AttackSpeedMultiplier = new(1f);
    public CurrentStatistic AttackDamageMultiplier = new(1f);
    public CurrentStatistic DodgeCooldown = new(1f);
    public CurrentStatistic DoubleJumpsCount = new(1f);
    public CurrentStatistic CanDodge = new(0f);
    // 0 means can dodge, 1 means can't dodge
    // DodgeCount is also possible, however, we are planning to have 1 dodge anyways, and doing it like this is 1 simple if check
    // instead if i would do a dodgeCount, it would be a lot more changes to make it all work
    
    public void Generate() {
        GlobalReference.PermanentPlayerStatistic.Generate();
        
        Speed.AddPermanentStats(GlobalReference.PermanentPlayerStatistic.Speed);
        JumpForce.AddPermanentStats(GlobalReference.PermanentPlayerStatistic.JumpForce);
        MaxHealth.AddPermanentStats(GlobalReference.PermanentPlayerStatistic.MaxHealth);
        AttackSpeedMultiplier.AddPermanentStats(GlobalReference.PermanentPlayerStatistic.AttackSpeedMultiplier);
        AttackDamageMultiplier.AddPermanentStats(GlobalReference.PermanentPlayerStatistic.AttackDamageMultiplier);
        DodgeCooldown.AddPermanentStats(GlobalReference.PermanentPlayerStatistic.DodgeCooldown);
        DoubleJumpsCount.AddPermanentStats(GlobalReference.PermanentPlayerStatistic.DoubleJumpsCount);
        CanDodge.AddPermanentStats(GlobalReference.PermanentPlayerStatistic.DoubleJumpsCount);
        CanDodge.Subscribe(() => AchievementManager.Grant("LEAP_OF_FAITH"));
    }
}
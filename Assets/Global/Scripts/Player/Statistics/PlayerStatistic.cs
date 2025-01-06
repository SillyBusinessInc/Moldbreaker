
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

    private int crumbs;
    public int Crumbs {
        get => crumbs;
        set => crumbs = value > 0 ? value : 0;
    }
    public List<string> Calories;
    public int CaloriesCount;
    public int CrumbsCount;
    public bool CollectedCrumb = false;
    public bool CollectedCalorie = false;
    // this is for the current stats of the player
    public CurrentStatistic Speed = new(12f);
    public CurrentStatistic JumpForce = new(8f);
    public CurrentStatistic MaxHealth = new(6f);
    public CurrentStatistic AttackSpeedMultiplier = new(1f);
    public CurrentStatistic AttackDamageMultiplier = new(1f);
    public CurrentStatistic DodgeCooldown = new(1f);
    public CurrentStatistic DoubleJumpsCount = new(1f);

    public void Generate() {
        GlobalReference.PermanentPlayerStatistic.Generate();
        
        Speed.AddPermanentStats(GlobalReference.PermanentPlayerStatistic.Speed);
        JumpForce.AddPermanentStats(GlobalReference.PermanentPlayerStatistic.JumpForce);
        MaxHealth.AddPermanentStats(GlobalReference.PermanentPlayerStatistic.MaxHealth);
        AttackSpeedMultiplier.AddPermanentStats(GlobalReference.PermanentPlayerStatistic.AttackSpeedMultiplier);
        AttackDamageMultiplier.AddPermanentStats(GlobalReference.PermanentPlayerStatistic.AttackDamageMultiplier);
        DodgeCooldown.AddPermanentStats(GlobalReference.PermanentPlayerStatistic.DodgeCooldown);
        DoubleJumpsCount.AddPermanentStats(GlobalReference.PermanentPlayerStatistic.DoubleJumpsCount);
    }
}
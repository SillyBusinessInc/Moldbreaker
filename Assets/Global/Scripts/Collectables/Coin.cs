using UnityEngine;

public class Coin : Collectable
{
    private int pointValue = 1; // Value of the coin

    public override void OnCollect()
    {
        var playerStats = GlobalReference.GetReference<PlayerReference>().Player.playerStatistic;
        playerStats.CollectedCrumb = true;
        playerStats.Crumbs += pointValue;
    }
}
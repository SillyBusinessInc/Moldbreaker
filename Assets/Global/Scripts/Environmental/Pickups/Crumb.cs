public class Crumb : PickupBase
{
    protected override void OnTrigger()
    {
        var playerStats = GlobalReference.GetReference<PlayerReference>().Player.playerStatistic;
        GlobalReference.GetReference<AudioManager>().PlaySFX("CrumbPickup");
        playerStats.CollectedCrumb = true;
        playerStats.Crumbs += 1;
    }
}

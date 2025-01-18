using UnityEngine;

public class HealthPickup : PickupBase
{
    protected override void OnTrigger() => GlobalReference.AttemptInvoke(Events.PICKUP_COLLECTED);
}

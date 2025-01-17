public abstract class Collectable : PickupBase
{
    // Called when the player collects this item
    public abstract void OnCollect();

    protected override void OnTrigger() => OnCollect();
}

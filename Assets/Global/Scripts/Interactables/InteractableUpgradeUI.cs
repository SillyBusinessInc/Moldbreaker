using System.Collections.Generic;

public class InteractableUpgradeUI : Interactable
{
    public List<UpgradeOption> upgradeOptions;

    public override void TriggerInteraction(PlayerInteraction interactor)
    {
        GlobalReference.GetReference<UpgradeOptions>().options = upgradeOptions;
        GlobalReference.GetReference<UpgradeOptions>().ShowOptions();
        
        base.TriggerInteraction(interactor);
        IsDisabled = true;
    }
}

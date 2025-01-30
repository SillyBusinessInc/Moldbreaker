using UnityEngine;

[CreateAssetMenu(menuName = "Actions/PanChanAction")]
public class PanChanAction : OneParamAction
{
    private static int totalClicks = 0;
    public override void InvokeAction(ActionMetaData _, string param)
    {
        GlobalReference.GetReference<AudioManager>().PlaySFX("PanClick");
        totalClicks += 1;
        if (totalClicks >= 69) AchievementManager.Grant("PAN_CHAN");
    }
}

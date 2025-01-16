using UnityEngine;

[CreateAssetMenu(menuName = "Actions/PanChanAction")]
public class PanChanAction : OneParamAction
{
    private int total_clicks = 0;
    [SerializeField] private string actionName = "Pan Chan Action";
    public override void InvokeAction(ActionMetaData _, string param)
    {
        AudioManager.Instance.PlaySFX("PainSFX");

        total_clicks += 1;
        if (total_clicks >= 69) AchievementManager.Grant("PAN_CHAN");
    }
}
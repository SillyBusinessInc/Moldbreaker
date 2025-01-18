using UnityEngine;

[CreateAssetMenu(menuName = "Actions/PanChanAction")]
public class PanChanAction : OneParamAction
{
    private int totalClicks = 0;
    public override void InvokeAction(ActionMetaData _, string param)
    {
        // Randomly choose between "PanClick1", "PanClick2", and "PanClick3"
        int randomIndex = Random.Range(1, 4);
        string randomSFX = $"PanClick{randomIndex}";

        AudioManager.Instance.PlaySFX(randomSFX);

        this.totalClicks += 1;
        if (this.totalClicks >= 69) AchievementManager.Grant("PAN_CHAN");
    }
}

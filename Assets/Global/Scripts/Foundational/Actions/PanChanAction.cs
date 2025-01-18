using UnityEngine;

[CreateAssetMenu(menuName = "Actions/PanChanAction")]
public class PanChanAction : OneParamAction
{
    private int total_clicks = 0;
    [SerializeField] private string actionName = "Pan Chan Action";
    public override void InvokeAction(ActionMetaData _, string param)
    {
        // Randomly choose between "PanClick1", "PanClick2", and "PanClick3"
        int randomIndex = Random.Range(1, 4);
        string randomSFX = $"PanClick{randomIndex}";

        AudioManager.Instance.PlaySFX(randomSFX);

        total_clicks += 1;
        if (total_clicks >= 69) AchievementManager.Grant("PAN_CHAN");
    }
}

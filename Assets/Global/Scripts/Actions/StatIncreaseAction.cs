using UnityEngine;
using System.Reflection;

public enum UpgradeType { Modify, Multiply }

[CreateAssetMenu(menuName = "Actions/StatIncreaseAction")]
public class StatIncreaseAction : ThreeParamAction
{
    [SerializeField] private PlayerStatistic playerStatistic;

    [Tooltip("The type of upgrade to apply to the stat")]
    [SerializeField] private UpgradeType param3;

    public override void InvokeAction(ActionMetaData _, string stat, string value, string upgradeType)
    {
        Player player = GlobalReference.GetReference<PlayerReference>().Player;
        UpgradeType upgrade = System.Enum.TryParse(upgradeType, true, out upgrade) ? upgrade : UpgradeType.Modify;

        if (player == null)
        {
            Debug.LogWarning("Player not found! [StatIncreaseAction]");
            return;
        }

        PlayerStatistic stats = player.playerStatistic;

        if (stats == null)
        {
            Debug.LogWarning("PlayerStatistic not found! [StatIncreaseAction]");
            return;
        }

        // Use reflection to find and modify the field
        FieldInfo field = typeof(PlayerStatistic).GetField(stat, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (field != null && field.FieldType == typeof(CurrentStatistic))
        {
            CurrentStatistic statistic = (CurrentStatistic)field.GetValue(stats);
            float valueFloat = float.Parse(value);

            if (upgrade == UpgradeType.Modify)
            {
                statistic.AddModifier("StatIncreaseAction", valueFloat);
            }
            else
            {
                statistic.AddMultiplier("StatIncreaseAction", valueFloat, false);
            }

            // check the new value
            Debug.Log($"New value of {stat}: {statistic.GetValue()}");
        }
        else
        {
            Debug.LogWarning("$[CONFIG] Field {param} not found in PlayerStatistic");
        }
    }
}
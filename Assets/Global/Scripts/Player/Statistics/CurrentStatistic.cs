using UnityEngine;
using System;

[Serializable]
public class CurrentStatistic : BaseStatistic
{
    [SerializeField]
    private float baseValue;  // Starting value, read-only
    public CurrentStatistic(float bv) {
        baseValue = bv;
    }

    // Get the final value after applying modifiers
    public float GetValue()
    {
        float value = baseValue;
        // first, apply the base multipliers
        value *= BaseMultipliers(1f); // add current
        // then, add the static modifiers
        modifiers.ForEach(pair => value += pair.Value);
        // lastly, combine the final multipliers
        value *= FinalMultipliers(1f);

        return value;
    }

    public int GetValueInt() => (int)MathF.Round(GetValue(), 0);
}
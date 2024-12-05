using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public class BaseStatistic
{
    // Lists of multipliers and modifiers with their associated keys
    protected List<KeyValuePair<string, float>> baseMultipliers = new();
    protected List<KeyValuePair<string, float>> finalMultipliers = new();
    protected List<KeyValuePair<string, float>> modifiers = new();

    // Event to notify listeners about changes
    public event Action OnChange;

    public float BaseMultipliers() => baseMultipliers.Any() ? baseMultipliers.Sum(pair => pair.Value) : 1;
    public float FinalMultipliers() => finalMultipliers.Any() ? finalMultipliers.Sum(pair => pair.Value) : 1;
    public List<KeyValuePair<string, float>> Modifiers() => modifiers;

    // Add new modifier  
    public void AddModifier(string key, float modifier)
    {
        if (modifier != 0) {
            modifiers.Add(new KeyValuePair<string, float>(key, modifier));
            TriggerOnChange();
        }
    }

    // Add new multiplier 
    public void AddMultiplier(string key, float multiplier, bool isBase)
    {
        if (multiplier != 0)
        {
            var pair = new KeyValuePair<string, float>(key, multiplier);
            if (isBase) baseMultipliers.Add(pair);
            else finalMultipliers.Add(pair);

            TriggerOnChange();
        }
    }

    // Remove a modifier by key
    public void RemoveModifier(string key)
    {
        if (modifiers.RemoveAll(pair => pair.Key == key) > 0)
            TriggerOnChange();
    }

    // Remove a multiplier by key
    public void RemoveMultiplier(string key, bool isBase)
    {
        bool removed = isBase
            ? baseMultipliers.RemoveAll(pair => pair.Key == key) > 0
            : finalMultipliers.RemoveAll(pair => pair.Key == key) > 0;

        if (removed) TriggerOnChange();
    }

    private void TriggerOnChange()
    {
        OnChange?.Invoke();
        GlobalReference.AttemptInvoke(Events.STATISTIC_CHANGED);
    }

    public void Subscribe(Action callback) => OnChange += callback;
    public void Unsubscribe(Action callback) => OnChange -= callback;
}
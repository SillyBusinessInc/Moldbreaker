using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class GlobalReference
{
    #region singletons monobehavior

    private static Statistics statistics;
    public static Statistics Statistics => statistics ??= new();

    private static Settings settings;
    public static Settings Settings => settings ??= new();
    
    private static AudioSettingSave audioSettingSave;
    public static AudioSettingSave AudioSettingSave => audioSettingSave ??= new();

    private static DevSettings devSettings;
    public static DevSettings DevSettings => devSettings ??= new();
    
    public static void Save()
    {
        
        statistics.SaveAll();
        settings.SaveAll();
        devSettings.SaveAll();
    }

    #endregion

    #region gameobject reference

    public static Dictionary<string, Reference> referenceList = new();

    public static void RegisterReference(Reference ref_)
    {
        if (!ref_)
        {
            Debug.LogWarning("Could not register object because no reference was given");
            return;
        }

        string name = ref_.GetType().Name;
        if (referenceList.ContainsKey(name))
        {
            Debug.LogError($"{name} is already assigned and can not be assigned multiple times. Please ensure there is only one {name} in this scene");
            return;
        }

        referenceList.Add(name, ref_);
    }

    public static void UnregisterReference(Reference ref_)
    {
        if (!ref_)
        {
            Debug.LogWarning("Could not unregister object because no reference was given");
            return;
        }

        string name = ref_.GetType().Name;
        if (!referenceList.ContainsKey(name))
        {
            Debug.LogWarning($"{name} cannot be unassigned because it has not been assigned in the first place");
            return;
        }

        referenceList.Remove(name);
    }

    /// <summary>
    /// <para>Finds the reference defined by the given generic type.</para>
    /// <para>Never use this method in Awake() or in OnDestroy()</para>
    /// </summary>
    public static T GetReference<T>()
    {
        string name = typeof(T).Name;
        if (referenceList.ContainsKey(name) && referenceList[name] is T t) return t;
        Debug.LogWarning($"Object '{name}' could not be found. Make sure to add this object to the scene exactly once");
        return default;
    }

    #endregion

    #region event logic

    public static Dictionary<Events, UnityEventBase> eventList = new();

    public static void SubscribeTo(Events eventName, UnityAction action) => TryGetEvent(eventName).AddListener(action);
    public static void SubscribeTo<T>(Events eventName, UnityAction<T> action) => TryGetEvent<T>(eventName).AddListener(action);

    public static void UnsubscribeTo(Events eventName, UnityAction action) => TryGetEvent(eventName).RemoveListener(action);
    public static void UnsubscribeTo<T>(Events eventName, UnityAction<T> action) => TryGetEvent<T>(eventName).RemoveListener(action);

    public static void AttemptInvoke(Events eventName)
    {
        // This log is allowed to stay :P, it's so useful
        Debug.Log($"Event Invoked ({eventName})");
        TryGetEvent(eventName).Invoke();
    }
    public static void AttemptInvoke<T>(Events eventName, T parameter)
    {
        // This log is allowed to stay :P, it's so useful
        Debug.Log($"Event Invoked ({eventName}), with parameter: ({parameter})");
        TryGetEvent<T>(eventName).Invoke(parameter);
    }

    private static UnityEvent TryGetEvent(Events eventName)
    {
        if (!eventList.ContainsKey(eventName)) return (UnityEvent)(eventList[eventName] = new UnityEvent());
        if (eventList[eventName] is UnityEvent e) return e;
        throw new UnityException($"You are trying to access {eventName} as if it was a parameterless event even though it has been created as an event with a parameter");
    }
    private static UnityEvent<T> TryGetEvent<T>(Events eventName)
    {
        if (!eventList.ContainsKey(eventName)) return (UnityEvent<T>)(eventList[eventName] = new UnityEvent<T>());
        if (eventList[eventName] is UnityEvent<T> e) return e;
        throw new UnityException($"You are trying to access {eventName} as if it was an event with a parameter even though it has been created as a parameterless event");
    }

    #endregion
}

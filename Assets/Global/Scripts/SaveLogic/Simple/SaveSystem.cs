using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class SaveSystem
{
    /// <summary> Defines the name of the save file </summary>
    protected abstract string Prefix {get;}
    /// <summary> The initialization method. Use the Add() methods in this method to initialize the content of this save file </summary>
    public abstract void Init();

    public SaveSystem() {
        Init();
        SaveAll();
    }

    // Logic
    private readonly Dictionary<string, ISaveable> saveables = new() {};

    /// <summary> Defines if this save file has unsaved changes </summary>
    public bool IsDirty { get; private set; } = false;
    public bool IsLocked { get; set; } = false;

    /// <summary> Finds the value with the given id </summary>
    public T Get<T>(string id) {
        Debug.Log($"get: {id}");
        // check if the value exists in this save system
        if (!saveables.ContainsKey(id)) {
            Debug.LogError($"{id} can not be found in {GetType().Name}");
            return default;
        }
        // return the requested value
        return (saveables[id] as Saveable<T>).Value;
    }

    /// <summary> Sets the value with the given id to a new value </summary>
    public void Set<T>(string id, T value) {
        Debug.Log($"set: {id}");
        if (IsLocked) return;
        // check if the value exists in this save system
        if (!saveables.ContainsKey(id)) {
            Debug.LogError($"{id} can not be found in {GetType().Name}");
            return;
        }
        // set value to requested value and make save system dirty
        saveables[id].Set(value);
        IsDirty = true;
    }

    /// <summary> Add a new value to the save system. This should generally be done in SaveSystem.Init() </summary>
    protected void Add<T>(string id, T defaultValue) {
        Debug.Log($"add: {id}");
        // check if type is allowed
        Type type = typeof(T);
        if (type != typeof(int) &&
            type != typeof(float) &&
            type != typeof(string) &&
            type != typeof(bool)
        ) Debug.LogError($"cannot save {id} because {type} is not a saveable type. please only save int, float, string or bool");
        // add value to the save system
        saveables.Add(id, new Saveable<T>($"{Prefix}_{id}", defaultValue));
        
    }

    /// <summary> Saves all data stored in the save system to PlayerPrefs </summary>
    public void SaveAll() {
        foreach (KeyValuePair<string, ISaveable> saveable in saveables) {
            saveable.Value.Save();
        }
        IsDirty = false;
    }

    /// <summary> Loads all data stored in PlayerPrefs to the save system </summary>
    public void LoadAll() {
        foreach (KeyValuePair<string, ISaveable> saveable in saveables) {
            saveable.Value.Load();
        }
        IsDirty = false;
    }

    /// <summary> Debug Method: Lists all values of the given type </summary>
    public void ListAll<T>() {
        foreach (KeyValuePair<string, ISaveable> saveable in saveables) {
            if (saveable.Value is Saveable<T> t) Debug.Log($"{t.Id}: {t.Value}");
        }
    }
}

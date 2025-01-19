using UnityEngine;

/// <summary> Non-generic interface for referencing saveable objects </summary>
public interface ISaveable {
    string Id {get; set;}
    public void Set<S>(S newValue);
    void Save();
    void Load();
}

/// <summary> Defines a saveable object of type T </summary>
public class Saveable<T> : ISaveable {
    public T Value { get; set; }
    public string Id { get; set; }

    public Saveable(string id, T defaultValue) {
        Id = id;

        if (PlayerPrefs.HasKey(id)) Load();
        else Value = defaultValue;
    }

    public void Set<S>(S newValue) {
        if (newValue is T t) Value = t;
    }
    /// <summary> Saves a value with PlayerPrefs according to it's type </summary>
    public void Save() {
        if (Value is int iValue) PlayerPrefs.SetInt(Id, iValue);
        if (Value is float fValue) PlayerPrefs.SetFloat(Id, fValue);
        if (Value is string sValue) PlayerPrefs.SetString(Id, sValue);
        if (Value is bool bValue) PlayerPrefs.SetInt(Id, bValue ? 1 : 0);
    }
    /// <summary> Loads a value from PlayerPrefs according to it's type </summary>
    public void Load() {
        if (Value is int) Value = (T)(object)PlayerPrefs.GetInt(Id);
        if (Value is float) Value = (T)(object)PlayerPrefs.GetFloat(Id);
        if (Value is string) Value = (T)(object)PlayerPrefs.GetString(Id);
        if (Value is bool) Value = (T)(object)(PlayerPrefs.GetInt(Id) == 1);
    }
}

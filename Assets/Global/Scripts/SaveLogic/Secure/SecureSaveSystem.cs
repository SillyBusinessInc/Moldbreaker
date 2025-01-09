using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public abstract class SecureSaveSystem
{
    /// <summary> Defines the name of the save file </summary>
    protected abstract string Prefix {get;}
    /// <summary> The initialization method. Use the Add() methods in this method to initialize the content of this save file </summary>
    public abstract void Init();

    public SecureSaveSystem() {
        Init();
        LoadAll();
    }

    // Logic
    private string SavePath => Path.Combine(Application.persistentDataPath, $"{Prefix}.mold");
    private readonly Dictionary<string, ISecureSaveable> saveables = new() {};

    /// <summary> Defines if this save file has unsaved changes </summary>
    public bool IsDirty { get; private set; } = false;
    public bool IsLocked { get; set; } = false;

    /// <summary> Finds the value with the given id </summary>
    public T Get<T>(string id) {
        // check if type is a seriallized object that requires special handling
        if (SerializableData.AllowedType(typeof(T)) == AllowedTypeReturnOption.ALLOWED_STRINGIFY) {
            return JsonConvert.DeserializeObject<SerializableWrapper<T>>(Get<string>(id)).Item;
        }
        // check if the value exists in this save system
        if (!saveables.ContainsKey(id)) {
            Debug.LogError($"{id} can not be found in {GetType().Name}");
            return default;
        }
        // return the requested value
        return (saveables[id] as SecureSaveable<T>).Value;
    }

    /// <summary> Sets the value with the given id to a new value </summary>
    public void Set<T>(string id, T value) {
        if (IsLocked) return;
        // check if type is a seriallized object that requires special handling
        if (SerializableData.AllowedType(typeof(T)) == AllowedTypeReturnOption.ALLOWED_STRINGIFY) {
            Set(id, JsonConvert.SerializeObject(new SerializableWrapper<T>(value)));
            return;
        }
        // check if the value exists in this save system
        if (!saveables.ContainsKey(id)) {
            Debug.LogError($"{id} can not be found in {GetType().Name}");
            return;
        }
        // set value to requested value and make save system dirty
        saveables[id].Set(value);
        IsDirty = true;
    }

    /// <summary> Add a new value to the save system. This should generally be done in SecureSaveSystem.Init() </summary>
    protected void Add<T>(string id, T defaultValue) {
        // check if type is allowed
        Type type = typeof(T);
        AllowedTypeReturnOption result = SerializableData.AllowedType(type);
        if (result == AllowedTypeReturnOption.DISALLOWED) Debug.LogError($"cannot save [{id}] because [{type}] is not a securely saveable type");
        // check if type is a seriallized object that requires special handling
        else if (result == AllowedTypeReturnOption.ALLOWED_STRINGIFY) {
            Add(id, JsonConvert.SerializeObject(new SerializableWrapper<T>(defaultValue)));
            return;
        }
        // add value to the save system
        saveables.Add(id, new SecureSaveable<T>($"{Prefix}_{id}", defaultValue));
    }

    /// <summary> Saves all data stored in the save system to a file </summary>
    public void SaveAll() {
        // open file to write to
        using (FileStream stream = new(SavePath, FileMode.OpenOrCreate)) {
            // create serializable data object for storing
            SerializableData data = new();
            // populate data object with values from the save system
            foreach (KeyValuePair<string, ISecureSaveable> saveable in saveables) {
                data.PullFromSecureSavable(saveable.Value);
            }
            // convert data object to file data for save storage
            BinaryWriter writer = new(stream);
            string jsonText = JsonConvert.SerializeObject(data);
            
            //to see the json text
            // Debug.Log(jsonText);

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(jsonText);
            // write data to file
            writer.Write(Convert.ToBase64String(plainTextBytes));
            writer.Flush();
        }
        IsDirty = false;
    }

    /// <summary> Loads all data stored in a file to the save system </summary>
    public void LoadAll() {
        if (File.Exists(SavePath)) {
            // open file to read from
            using (FileStream stream = new(SavePath, FileMode.Open)) {
                // convert file data to data object for loading
                BinaryReader reader = new(stream);
                byte[] encodedBytes = Convert.FromBase64String(reader.ReadString());
                SerializableData data = JsonConvert.DeserializeObject<SerializableData>(Encoding.UTF8.GetString(encodedBytes));
                // populate current save system with values from data object
                foreach (KeyValuePair<string, ISecureSaveable> saveable in saveables) {
                    data.PushToSecureSaveable(saveable.Value);
                }
            }
            IsDirty = false;
        }
    }

    /// <summary> Debug Method: Lists all values of the given type </summary>
    public void ListAll<T>() {
        foreach (KeyValuePair<string, ISecureSaveable> saveable in saveables) {
            if (saveable.Value is SecureSaveable<T> t) Debug.Log($"[{Prefix}] {t.Id}: [{t.Value.GetType().Name}] {t.Value}");
        }
    }

    [Serializable]
    public class SerializableWrapper<T> {
        public T Item;
        public SerializableWrapper(T item_) {
            Item = item_;
        }
    }
}

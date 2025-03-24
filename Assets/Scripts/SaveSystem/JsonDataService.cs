using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class JsonDataService : IDataService
{
    public bool SaveData<T>(string relativePath, T data, bool encrypted){
        string path = Application.persistentDataPath + relativePath;

        try{
            if (File.Exists(path)){
                Debug.Log("Data exists. Deleting old file and writing a new one");
                File.Delete(path);
            }
            using FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, JsonConvert.SerializeObject(data));
            Debug.Log("Data saved: path: " + path);
            return true;
        }
        catch (Exception e){
            Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
            return false;
        }
    }

    public T LoadData<T>(string relativePath, bool encrypted){
        string path = Application.persistentDataPath + relativePath;

        if (!File.Exists(path)){
            throw new FileNotFoundException($"{path} does not exist");
        }

        try{
            T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            return data;
        }
        catch (Exception e){
            Debug.LogError($"Unable to load data due to: {e.Message} {e.StackTrace}");
            throw e;
        }
    }
}

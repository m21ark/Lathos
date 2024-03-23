using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void DataSave(Transform dataToSave)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/data.sav";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(dataToSave);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData DataLoad()
    {
        string path = Application.persistentDataPath + "/data.sav";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void DeleteSave()
    {
        string path = Application.persistentDataPath + "/data.sav";
        if (File.Exists(path)) File.Delete(path);
        else Debug.Log("Save file not found in " + path);
    }

    public static bool SaveExists()
    {
        string path = Application.persistentDataPath + "/data.sav";
        return File.Exists(path);
    }
}
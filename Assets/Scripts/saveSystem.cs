using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class saveSystem
{
    private static string path = Application.persistentDataPath + "/gameProgress.colour";

    public static void saveProgress(int l, bool[] com, int v)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        gameData data = new gameData(l, com, v);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static gameData loadProgress()
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            gameData data = formatter.Deserialize(stream) as gameData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in the " + path);
            return null;
        }
    }
}

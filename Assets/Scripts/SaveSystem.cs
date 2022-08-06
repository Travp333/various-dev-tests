using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
//Made by Andrew Rodriguez with the help of https://www.youtube.com/watch?v=XOjd_qU2Ido&ab_channel=Brackeys
public static class SaveSystem
{
    public static void SavePlayer(PlayerStats player)
    {
        //Debug.Log("save player called");
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.txt";
        FileStream stream = new FileStream(path, FileMode.Create);
        
        PlayerData data = new PlayerData(player);
        formatter.Serialize(stream, data);
        stream.Close();
    }


    public static PlayerData LoadPlayerStats()
    {
        //Debug.Log("load player called");
        string path = Application.persistentDataPath + "/player.txt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}

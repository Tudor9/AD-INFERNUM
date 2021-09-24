using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    // contine logica de salvare a starii jocului intr-un fisier codat binar
    public static void SavePlayer(Player player, string[] entityNames)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(Application.persistentDataPath + "/" + 
            AdInfernumDb.username + "/playerData" + MenuFunctions.saveFile + ".sav", 
            FileMode.Create);

        PlayerData data = new PlayerData(player, entityNames);

        binaryFormatter.Serialize(fileStream, data);
        fileStream.Close();
    }

    // contine logica pentru a returna datele salvate dintr-un fisier codat binar
    public static PlayerData LoadPlayer()
    {
        if (File.Exists(Application.persistentDataPath + "/" + 
            AdInfernumDb.username + "/playerData" + MenuFunctions.saveFile + ".sav"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(Application.persistentDataPath + "/" + 
                AdInfernumDb.username + "/playerData" + MenuFunctions.saveFile + ".sav", 
                FileMode.Open);

            PlayerData playerData = binaryFormatter.Deserialize(fileStream) as PlayerData;

            fileStream.Close();

            return playerData;
        } 
        else 
        {
            Debug.LogError("Save file not found. Path: " + Application.persistentDataPath);
            return null;
        }
    }
}
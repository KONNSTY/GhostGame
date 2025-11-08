using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static string SavePath => Application.persistentDataPath + "/player.save";

    // Bestehende Save-Methode
    public static void SavePlayer(GameMode gamemode, GameObject player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(SavePath, FileMode.Create);

        PlayerData data = new PlayerData(gamemode, player);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Spiel gespeichert in: " + SavePath);
    }

    // Bestehende Load-Methode
    public static PlayerData Loadgame()
    {
        if (File.Exists(SavePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(SavePath, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            
            Debug.Log("Spiel geladen von: " + SavePath);
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + SavePath);
            return null;
        }
    }

    // ✅ NEU: Prüfen ob Savegame existiert
    public static bool HasSaveFile()
    {
        return File.Exists(SavePath);
    }

    // ✅ NEU: Savegame löschen (für New Game)
    public static void DeleteSaveFile()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("Altes Savegame gelöscht: " + SavePath);
        }
        else
        {
            Debug.Log("Kein Savegame zum Löschen gefunden.");
        }
    }

    // ✅ NEU: Neues Spiel starten (löscht altes Savegame)
    public static void StartNewGame(GameMode gamemode, GameObject player)
    {
        // Altes Savegame löschen
        DeleteSaveFile();
        
        // Neues Savegame erstellen
        SavePlayer(gamemode, player);
        
        Debug.Log("Neues Spiel gestartet und gespeichert!");
    }
}


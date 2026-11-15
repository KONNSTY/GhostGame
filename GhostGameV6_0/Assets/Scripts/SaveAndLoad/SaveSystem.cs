using System;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string SavePath => Application.persistentDataPath + "/player.save";

    public static void SavePlayer(GameMode gamemode, GameObject player)
    {
        if (gamemode == null || player == null)
        {
            Debug.LogError("Save aborted: missing game mode or player.");
            return;
        }

        PlayerData data = new PlayerData(gamemode, player);
        if (data.position == null || data.position.Length < 3)
            return;

        var save = new GhostSaveData
        {
            Mission = (int)data.type,
            X = data.position[0],
            Y = data.position[1],
            Z = data.position[2],
            Keys = data.KeyName ?? Array.Empty<string>(),
            Notes = data.NoteName ?? Array.Empty<string>()
        };

        if (!GhostSaveCodec.TryEncode(save, out byte[] bytes))
        {
            Debug.LogError("Save aborted: player data was rejected.");
            return;
        }

        string tempPath = SavePath + ".tmp";
        try
        {
            File.WriteAllBytes(tempPath, bytes);
            File.Copy(tempPath, SavePath, true);
            Debug.Log("Spiel gespeichert.");
        }
        catch (Exception)
        {
            Debug.LogError("Save aborted: the save file could not be written.");
        }
        finally
        {
            try
            {
                if (File.Exists(tempPath))
                    File.Delete(tempPath);
            }
            catch (Exception)
            {
                Debug.LogWarning("Temporary save file could not be removed.");
            }
        }
    }

    public static PlayerData Loadgame()
    {
        if (!TryReadSaveBytes(out byte[] bytes))
        {
            Debug.LogError("Save file not found or was rejected.");
            return null;
        }

        if (!GhostSaveCodec.TryDecode(bytes, out GhostSaveData save))
        {
            Debug.LogError("Save file was rejected.");
            return null;
        }

        Debug.Log("Spiel geladen.");
        return PlayerData.FromLoaded(save);
    }

    public static bool HasSaveFile()
    {
        if (!TryReadSaveBytes(out byte[] bytes))
            return false;

        return GhostSaveCodec.TryDecode(bytes, out _);
    }

    public static void DeleteSaveFile()
    {
        try
        {
            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
                Debug.Log("Altes Savegame gelöscht.");
            }
            else
            {
                Debug.Log("Kein Savegame zum Löschen gefunden.");
            }
        }
        catch (Exception)
        {
            Debug.LogError("Save file could not be deleted.");
        }
    }

    public static void StartNewGame(GameMode gamemode, GameObject player)
    {
        DeleteSaveFile();
        SavePlayer(gamemode, player);
        Debug.Log("Neues Spiel gestartet und gespeichert!");
    }

    static bool TryReadSaveBytes(out byte[] bytes)
    {
        bytes = null;
        try
        {
            if (!File.Exists(SavePath))
                return false;

            var info = new FileInfo(SavePath);
            if (info.Length <= 0 || info.Length > GhostSaveCodec.MaxBytes)
                return false;

            bytes = File.ReadAllBytes(SavePath);
            return bytes.Length > 0 && bytes.Length <= GhostSaveCodec.MaxBytes;
        }
        catch (Exception)
        {
            bytes = null;
            return false;
        }
    }
}

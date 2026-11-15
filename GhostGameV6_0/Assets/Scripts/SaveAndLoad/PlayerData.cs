using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public GameMode.MissionType type;
    public float[] position;
    public string[] KeyName;
    public string[] NoteName;
    public int[] KeyID;
    public int[] NoteID;

    public PlayerData()
    {
    }

    public static PlayerData FromLoaded(GhostSaveData save)
    {
        var data = new PlayerData();
        data.type = (GameMode.MissionType)save.Mission;
        data.position = new float[] { save.X, save.Y, save.Z };
        data.KeyName = save.Keys ?? new string[0];
        data.NoteName = save.Notes ?? new string[0];
        data.KeyID = IndexIds(data.KeyName.Length);
        data.NoteID = IndexIds(data.NoteName.Length);
        return data;
    }

    static int[] IndexIds(int length)
    {
        int[] ids = new int[length];
        for (int i = 0; i < length; i++)
            ids[i] = i;
        return ids;
    }

    public PlayerData(GameMode gameMode, GameObject playerObject = null)
    {
        // Wenn kein Player übergeben wurde, automatisch suchen
        if (playerObject == null)
        {
            playerObject = FindPlayerObject();
        }
        
        if (playerObject != null && gameMode != null)
        {
            // Mission speichern
            type = gameMode.currentMIssion;
            
            // Position speichern
            position = new float[3];
            position[0] = playerObject.transform.position.x;
            position[1] = playerObject.transform.position.y;
            position[2] = playerObject.transform.position.z;

            // Inventory-Daten speichern
            SaveInventoryData(playerObject);
        }
        else
        {
            Debug.LogError("Player oder GameMode nicht gefunden!");
        }
    }

    GameObject FindPlayerObject()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
            return player;

        PlayerController playerController = UnityEngine.Object.FindAnyObjectByType<PlayerController>();
        if (playerController != null)
            return playerController.gameObject;

        PlayerInventory inventory = UnityEngine.Object.FindAnyObjectByType<PlayerInventory>();
        if (inventory != null)
            return inventory.gameObject;

        return null;
    }

    void SaveInventoryData(GameObject playerObject)
    {
        PlayerInventory inventory = playerObject.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            // Keys
            if (inventory.inventoryListKey != null && inventory.inventoryListKey.Count > 0)
            {
                KeyName = inventory.inventoryListKey.ToArray();
                KeyID = new int[KeyName.Length];
                for (int i = 0; i < KeyID.Length; i++)
                {
                    KeyID[i] = i;
                }
            }
            else
            {
                KeyName = new string[0];
                KeyID = new int[0];
            }

            // Notes
            if (inventory.inventoryListNote != null && inventory.inventoryListNote.Count > 0)
            {
                NoteName = inventory.inventoryListNote.ToArray();
                NoteID = new int[NoteName.Length];
                for (int i = 0; i < NoteID.Length; i++)
                {
                    NoteID[i] = i;
                }
            }
            else
            {
                NoteName = new string[0];
                NoteID = new int[0];
            }
        }
        else
        {
            Debug.LogWarning("PlayerInventory Component nicht gefunden auf " + playerObject.name);
        }
    }
}
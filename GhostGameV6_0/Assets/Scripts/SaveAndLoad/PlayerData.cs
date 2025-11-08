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

    public PlayerData(GameMode gameMode, GameObject playerObject = null)
    {
        // Wenn kein Player übergeben wurde, automatisch suchen
        if (playerObject == null)
        {
            playerObject = FindPlayerObject();
        }
        
        if (playerObject != null)
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
            Debug.LogError("Player GameObject nicht gefunden!");
        }
    }

    GameObject FindPlayerObject()
    {
        // Methode 1: Über Tag
        GameObject player = GameObject.Find("Player");
        if (player != null) return player;



        // Methode 3: Über Component
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null) return playerController.gameObject;

        // Methode 4: Über PlayerInventory Component
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        if (inventory != null) return inventory.gameObject;

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
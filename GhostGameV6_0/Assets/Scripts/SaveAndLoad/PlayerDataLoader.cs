using UnityEngine;

public class PlayerDataLoader : MonoBehaviour
{
    [Header("References")]
    public PlayerController playerController;
    public GameObject gmObJ;
    public GameMode gameMode;

    [Header("Spawn Point System")]
    public Transform newGameSpawnPoint;
    public bool useCurrentPositionAsSpawn = false;

    [Header("Debug")]
    public bool enableDebugLogs = true;

    void Start()
    {
        FindReferences();
        Invoke(nameof(InitializePlayerPosition), 0.2f);
    }

    void FindReferences()
    {
        if (playerController == null)
            playerController = GetComponent<PlayerController>();

        if (gmObJ == null)
            gmObJ = GameObject.Find("GameMode");

        if (gameMode == null && gmObJ != null)
            gameMode = gmObJ.GetComponent<GameMode>();

        if (newGameSpawnPoint == null)
        {
            GameObject spawnPointObj = GameObject.Find("NewGameSpawnPoint");
            if (spawnPointObj != null)
            {
                newGameSpawnPoint = spawnPointObj.transform;
                Log("Spawn Point automatisch gefunden: " + spawnPointObj.name);
            }
        }
    }

    void InitializePlayerPosition()
    {
        int isNewGame = PlayerPrefs.GetInt("IsNewGame", 1);
        Log($"Initialisiere Player Position - IsNewGame: {isNewGame}");

        StopPlayerMovement();

        if (isNewGame == 0)
        {
            LoadPlayerPosition();
        }
        else
        {
            SetNewGamePosition();
        }

        Invoke(nameof(RestorePlayerMovement), 0.3f);
    }

    void SetNewGamePosition()
    {
        Vector3 spawnPosition;

        if (newGameSpawnPoint != null)
        {
            spawnPosition = newGameSpawnPoint.position;
            Log($"🎮 NEW GAME - Verwende Spawn Point: {spawnPosition}");
        }
        else if (useCurrentPositionAsSpawn)
        {
            spawnPosition = transform.position;
            Log($"🎮 NEW GAME - Verwende aktuelle Position: {spawnPosition}");
        }
        else
        {
            spawnPosition = new Vector3(0, 1, 0);
            LogWarning("⚠️ Kein Spawn Point gefunden - verwende Fallback Position (0,1,0)");
        }

        ForcePlayerPosition(spawnPosition);
        SetupNewGameState();
        Log($"✅ NEW GAME Setup abgeschlossen - Player Position: {transform.position}");
    }

    void LoadPlayerPosition()
    {
        PlayerData loadedData = SaveSystem.Loadgame();

        if (loadedData != null && loadedData.position != null && loadedData.position.Length >= 3)
        {
            Vector3 loadedPosition = new Vector3(
                loadedData.position[0],
                loadedData.position[1],
                loadedData.position[2]
            );

            Log($"📂 LOAD GAME - Position aus Savegame: {loadedPosition}");
            ForcePlayerPosition(loadedPosition);
            LoadGameState(loadedData);
            Log($"✅ LOAD GAME abgeschlossen - Player Position: {transform.position}");
        }
        else
        {
            LogWarning("❌ Fehlerhafte Savegame-Daten - starte New Game");
            SetNewGamePosition();
        }
    }

    void ForcePlayerPosition(Vector3 targetPosition)
    {
        Log($"🎯 Setze Position von {transform.position} zu {targetPosition}");

        transform.position = targetPosition;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.position = targetPosition;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.Sleep();
            rb.WakeUp();
        }

        CharacterController cc = GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
            transform.position = targetPosition;
            cc.enabled = true;
        }

        if (newGameSpawnPoint != null && PlayerPrefs.GetInt("IsNewGame", 1) == 1)
        {
            transform.rotation = newGameSpawnPoint.rotation;
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }

        // ✅ KORRIGIERT: Proper method calls statt Lambda
        Invoke(nameof(RepeatPositionSet1), 0.1f);
        Invoke(nameof(RepeatPositionSet2), 0.2f);
    }

    // ✅ KORRIGIERT: Separate Methoden für Position-Wiederholung
    void RepeatPositionSet1()
    {
        RepeatPositionSet(GetTargetPosition());
    }

    void RepeatPositionSet2()
    {
        RepeatPositionSet(GetTargetPosition());
    }

    Vector3 GetTargetPosition()
    {
        // Target Position basierend auf Game State
        int isNewGame = PlayerPrefs.GetInt("IsNewGame", 1);
        
        if (isNewGame == 1) // New Game
        {
            if (newGameSpawnPoint != null)
                return newGameSpawnPoint.position;
            else if (useCurrentPositionAsSpawn)
                return transform.position;
            else
                return new Vector3(0, 1, 0);
        }
        else // Load Game - verwende aktuelle Position als Ziel
        {
            return transform.position;
        }
    }

    void RepeatPositionSet(Vector3 position)
    {
        if (Vector3.Distance(transform.position, position) > 0.1f)
        {
            transform.position = position;
            
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.position = position;
                rb.linearVelocity = Vector3.zero;
            }
            
            Log($"🔄 Position erneut korrigiert: {position}");
        }
    }

    void StopPlayerMovement()
    {
        if (playerController != null)
        {
            playerController.enabled = false;
            Log("PlayerController gestoppt");
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }

    void RestorePlayerMovement()
    {
        if (playerController != null)
        {
            playerController.enabled = true;
            Log("PlayerController wieder aktiviert");
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        Log($"🎮 Movement wiederhergestellt - Finale Position: {transform.position}");
    }

    void SetupNewGameState()
    {
        if (gameMode != null)
        {
            gameMode.currentMIssion = GameMode.MissionType.Mission0;
            Log($"Mission auf Start gesetzt: {GameMode.MissionType.Mission0}");
        }

        ClearInventory();
        SaveInitialGameState();
    }

    void LoadGameState(PlayerData data)
    {
        if (gameMode != null)
        {
            gameMode.currentMIssion = data.type;
            Log($"Mission geladen: {data.type}");
        }

        LoadInventoryData(data);
    }

    void LoadInventoryData(PlayerData data)
    {
        PlayerInventory inventory = GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            inventory.inventoryListKey.Clear();
            inventory.inventoryListNote.Clear();

            if (data.KeyName != null)
            {
                for (int i = 0; i < data.KeyName.Length; i++)
                {
                    inventory.inventoryListKey.Add(data.KeyName[i]);
                }
                Log($"Schlüssel geladen: {data.KeyName.Length}");
            }

            if (data.NoteName != null)
            {
                for (int i = 0; i < data.NoteName.Length; i++)
                {
                    inventory.inventoryListNote.Add(data.NoteName[i]);
                }
                Log($"Notizen geladen: {data.NoteName.Length}");
            }
        }
    }

    void ClearInventory()
    {
        PlayerInventory inventory = GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            inventory.inventoryListKey.Clear();
            inventory.inventoryListNote.Clear();
            Log("Inventar geleert");
        }
    }

    void SaveInitialGameState()
    {
        if (gameMode != null)
        {
            Invoke(nameof(DoInitialSave), 0.1f);
        }
    }

    void DoInitialSave()
    {
        SaveSystem.SavePlayer(gameMode, gameObject);
        Log($"💾 Erstes Savegame erstellt - Position: {transform.position}");
    }

    public void SaveCurrentState()
    {
        if (gameMode != null)
        {
            SaveSystem.SavePlayer(gameMode, gameObject);
            Log($"💾 Spiel gespeichert - Position: {transform.position}");
        }
    }

    [ContextMenu("Set Current Position as New Game Spawn")]
    void SetCurrentPositionAsSpawn()
    {
        if (newGameSpawnPoint == null)
        {
            GameObject spawnObj = new GameObject("NewGameSpawnPoint");
            newGameSpawnPoint = spawnObj.transform;
        }
        
        newGameSpawnPoint.position = transform.position;
        newGameSpawnPoint.rotation = transform.rotation;
        
        Log($"🎯 Spawn Point gesetzt auf: {newGameSpawnPoint.position}");
    }

    void Log(string message)
    {
        if (enableDebugLogs)
            Debug.Log($"[PlayerDataLoader] {message}");
    }

    void LogWarning(string message)
    {
        if (enableDebugLogs)
            Debug.LogWarning($"[PlayerDataLoader] {message}");
    }
}
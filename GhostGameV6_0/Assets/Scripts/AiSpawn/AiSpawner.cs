using Unity.Mathematics;
using UnityEngine;

public class AiSpawner : MonoBehaviour
{
    public bool isGoal9ActiveAndNoGhostsShouldSpawn = false;
    public GameObject gameModeObj;
    public GameMode gameMode;

    public bool canOnlySpawnOnce = false;
    private bool isFirstSpawn = true; // ✅ FIX: Track ob dies der erste Spawn ist

    private GameObject CircleWallInstance;
    public GameObject player;

    public GameObject aiPrefab;
    private GameObject circleWallPrefab;

    public bool bFinalStarts = false;

    public float DistanceToPlayer;

    public bool canSpawnAi = false;
    public bool isAllowingAiSpawn = true; // Flag to control AI spawning (Tippfehler behoben)
    
    public GameObject[] spawnedAIs; // Track gespawnte AIs

    public bool DestroyAllAibyProgrammer = false;

    public bool NoAiActive = false; // Flag to control if AI should be spawned

    public float MaxDistance;
    public float MinDistance;

    private WeaponInLight WeaponInLight;
    
    void Start()
    {
        gameModeObj = GameObject.Find("GameMode");
        gameMode = gameModeObj.GetComponent<GameMode>();
        aiPrefab = Resources.Load<GameObject>("Ghosts");
        circleWallPrefab = Resources.Load<GameObject>("CircleWall Variant");
        
        // ✅ FIX: Debug-Check für CircleWall Prefab
        if (circleWallPrefab == null)
        {
            Debug.LogError("❌ CircleWall Prefab nicht gefunden! Überprüfe Resources/CircleWall Variant");
        }
        else
        {
            Debug.Log($"✅ CircleWall Prefab geladen: {circleWallPrefab.name}");
        }
        WeaponInLight = player.GetComponentInChildren<WeaponInLight>();
        MaxDistance = 10f;
        MinDistance = 5f; // ✅ FIX: MinDistance war nicht initialisiert
    }
    
    void Update()
    {
        // Check if any active AIs exist
        NoAiActive = true;
        if (spawnedAIs != null)
        {
            for (int i = 0; i < spawnedAIs.Length; i++)
            {
                if (spawnedAIs[i] != null)
                {
                    NoAiActive = false;
                    break;
                }
            }
        }

        if (gameMode.shouldAiSpawnerAllDeactivate == false)
        {
            DistanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            // ✅ Spawning Logic
            if (DistanceToPlayer < MinDistance && isAllowingAiSpawn == true && canOnlySpawnOnce == false)
            {
                canSpawnAi = true;
                isAllowingAiSpawn = false;
                Debug.Log($"🎯 Spawn-Trigger aktiviert! Distanz: {DistanceToPlayer:F2} < {MinDistance}");
            }
            else
            {
                canSpawnAi = false;
            }

            // ✅ KORRIGIERT: Kill Count Check mit Aktion - basiert auf tatsächlich gespawnten AIs
            int requiredKills = (spawnedAIs != null) ? spawnedAIs.Length : gameMode.aiSpawners;
            if (WeaponInLight != null && WeaponInLight.killCount >= requiredKills && CircleWallInstance != null)
            {
                Debug.Log($"🎯 Kill Count erreicht: {WeaponInLight.killCount}");
                Debug.Log("✅ Zerstöre CircleWall...");

                Destroy(CircleWallInstance);
                CircleWallInstance = null;
                player.GetComponent<PlayerController>().health = 100;
                WeaponInLight.killCount = 0;
                isAllowingAiSpawn = true; // Erlaube neues Spawning
                canOnlySpawnOnce = false; // ✅ FIX: Erlaube neues Spawning für nächste Runde
            }

            // ✅ AI Distance Management (nur wenn spawnedAIs existiert)
            if (spawnedAIs != null && spawnedAIs.Length > 0)
            {
                for (int i = 0; i < spawnedAIs.Length; i++)
                {
                    if (spawnedAIs[i] != null)
                    {
                        float distanceToAI = Vector3.Distance(spawnedAIs[i].transform.position, player.transform.position);

                        if (distanceToAI > MaxDistance)
                        {
                            AiController aiController = spawnedAIs[i].GetComponent<AiController>();
                            if (aiController != null)
                            {
                                aiController.state = AiController.AiState.WarpToPlayer;
                            }
                        }
                    }
                }
            }

            // ✅ AI Spawning
            if (canSpawnAi == true)
            {
                SpawnAi();
                canSpawnAi = false;
            }
            else if (isGoal9ActiveAndNoGhostsShouldSpawn == true)
            {
                DestroySpawnedAIs();
            }
        }

        // ✅ Debug Destroy
        if (DestroyAllAibyProgrammer == true)
        {
            DestroyAllAIs();
            DestroyAllAibyProgrammer = false; // Reset flag
        }
    }
    
    void DestroyAllAIs()
    {
        if (spawnedAIs != null)
        {
            for (int i = 0; i < spawnedAIs.Length; i++)
            {
                if (spawnedAIs[i] != null)
                {
                    Destroy(spawnedAIs[i]);
                }
            }
            spawnedAIs = null;
        }
        
        if (CircleWallInstance != null)
        {
            Destroy(CircleWallInstance);
            CircleWallInstance = null;
        }
        
    
        isAllowingAiSpawn = true;
        canOnlySpawnOnce = false; // ✅ FIX: Erlaube neues Spawning
        // Hinweis: isFirstSpawn wird NICHT zurückgesetzt - nur beim echten Level-Reset
    }

    public void SpawnAi()
    {
        // ✅ FIX: Null-Check für aiSpawners
        if (gameMode.aiSpawners <= 0)
        {
            Debug.LogWarning("aiSpawners ist 0 oder negativ!");
            return;
        }

        // ✅ FIX: Check if aiPrefab is valid
        if (aiPrefab == null)
        {
            Debug.LogError("aiPrefab ist null! Kann keine AIs spawnen.");
            return;
        }

        // ✅ FIX: Erstes Spawn = 1 Geist, nachfolgende = normale Anzahl
        int spawnCount = isFirstSpawn ? 1 : gameMode.aiSpawners;
        Debug.Log($"🎯 Spawne {spawnCount} Geister (Erstes Spawn: {isFirstSpawn})");
        
        // Array für gespawnte AIs initialisieren
        spawnedAIs = new GameObject[spawnCount];

        if (spawnCount > 7) // ✅ FIX: Basiert auf tatsächlicher Spawn-Anzahl
        {
            bFinalStarts = true;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 SpherePos = UnityEngine.Random.insideUnitSphere * 20f;
            SpherePos.y = 0f;
            Vector3 spawnPosition = player.transform.position + SpherePos;

            // AI spawnen und Referenz speichern
            GameObject spawnedAI = Instantiate(aiPrefab, spawnPosition, Quaternion.identity);
            spawnedAIs[i] = spawnedAI;
        }

        // ✅ FIX: Nach dem ersten Spawn, erlaube mehr Geister
        if (isFirstSpawn)
        {
            isFirstSpawn = false;
            Debug.Log("✅ Erstes Spawn abgeschlossen - nächste Spawns haben mehr Geister!");
        }
        
        // ✅ FIX: Circle Wall NACH AI-Spawn erstellen
        if (circleWallPrefab != null && CircleWallInstance == null)
        {
            CircleWallInstance = Instantiate(circleWallPrefab,
                                           player.transform.position,
                                           quaternion.identity);
            Debug.Log($"🛑 Circle Wall gespawnt an Position: {player.transform.position}");
        }
        else if (circleWallPrefab == null)
        {
            Debug.LogError("❌ Kann Circle Wall nicht spawnen - Prefab ist null!");
        }
        else if (CircleWallInstance != null)
        {
            Debug.LogWarning("⚠️ Circle Wall bereits vorhanden!");
        }
        
        canOnlySpawnOnce = true; // Verhindert mehrfaches Spawnen
    }

    // Neue Methode zum Zerstören gespawnter AIs
    public void DestroySpawnedAIs()
    {
        if (spawnedAIs != null && spawnedAIs.Length > 0)
        {
            // ✅ FIX: Vereinfacht - Ihr ursprünglicher Code war zu kompliziert
            for (int i = 0; i < spawnedAIs.Length; i++)
            {
                if (spawnedAIs[i] != null)
                {
                    Destroy(spawnedAIs[i]);
                }
            }

            spawnedAIs = null; // Array zurücksetzen
            NoAiActive = true; // ✅ FIX: Status korrekt setzen
        }
        else
        {
            NoAiActive = true;
        }
    }
}


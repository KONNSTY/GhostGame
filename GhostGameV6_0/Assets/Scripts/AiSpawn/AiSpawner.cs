using Unity.Mathematics;
using UnityEngine;

public class AiSpawner : MonoBehaviour
{
    public bool isGoal9ActiveAndNoGhostsShouldSpawn = false;
    public GameObject gameModeObj;
    public GameMode gameMode;

    public bool canOnlySpawnOnce = false;

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
        
                if (circleWallPrefab != null)
                {
                    CircleWallInstance = Instantiate(circleWallPrefab,
                                                   player.transform.position,
                                                   quaternion.identity);
                }
            }
            else
            {
                canSpawnAi = false;
            }

            // ✅ KORRIGIERT: Kill Count Check mit Aktion
            if (WeaponInLight != null && WeaponInLight.killCount >= gameMode.aiSpawners && CircleWallInstance != null)
            {
                Debug.Log($"🎯 Kill Count erreicht: {WeaponInLight.killCount}");
                Debug.Log("✅ Zerstöre CircleWall...");

                Destroy(CircleWallInstance);
                CircleWallInstance = null;
                player.GetComponent<PlayerController>().health = 100;
                WeaponInLight.killCount = 0;
                isAllowingAiSpawn = true; // Erlaube neues Spawning
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

        // Array für gespawnte AIs initialisieren
        spawnedAIs = new GameObject[gameMode.aiSpawners];

        if (spawnedAIs.Length > 7)
        {
            bFinalStarts = true;
        }

        for (int i = 0; i < gameMode.aiSpawners; i++)
        {
            Vector3 SpherePos = UnityEngine.Random.insideUnitSphere * 20f;
            SpherePos.y = 0f;
            Vector3 spawnPosition = player.transform.position + SpherePos;

            // AI spawnen und Referenz speichern
            GameObject spawnedAI = Instantiate(aiPrefab, spawnPosition, Quaternion.identity);
            spawnedAIs[i] = spawnedAI;
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


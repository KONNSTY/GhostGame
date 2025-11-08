using UnityEngine;

public class Goal : MonoBehaviour
{
    public Transform GoalPoint;
    public Transform player;

    public GameObject gamemode;
    public GameMode gm;

    public GameObject[] Goals = new GameObject[12];
    
    // Cache für bessere Performance
    private GameMode.MissionType lastMission = GameMode.MissionType.Mission0;
    private bool hasCheckedInitialMission = false;

    void Start()
    {
        // Null-Prüfung für GameMode
        if (gamemode != null)
        {
            gm = gamemode.GetComponent<GameMode>();
            if (gm == null)
            {
                Debug.LogError("GameMode Component nicht gefunden auf " + gamemode.name);
            }
        }
        else
        {
            Debug.LogError("GameMode GameObject ist nicht zugewiesen!");
        }
        
        // Initiale Mission setzen
        UpdateGoalPoint();
    }

    void Update()
    {
        // Player-Position folgen
        if (player != null)
        {
            transform.position = new Vector3(player.position.x, player.position.y + 0.5f, player.position.z);
        }

        // Goal-Update nur wenn Mission geändert hat (Performance-Optimierung)
        if (gm != null && (gm.currentMIssion != lastMission || !hasCheckedInitialMission))
        {
            UpdateGoalPoint();
            lastMission = gm.currentMIssion;
            hasCheckedInitialMission = true;
        }

        // Rotation zum Goal (nur wenn GoalPoint existiert)
        RotateTowardsGoal();
    }

    void UpdateGoalPoint()
    {
        if (gm == null) return;

        // Konvertiere MissionType zu Array-Index
        int missionIndex = (int)gm.currentMIssion;
        
        // Prüfe ob Mission-Index valid ist und Goal existiert
        if (missionIndex >= 0 && missionIndex < Goals.Length && Goals[missionIndex] != null)
        {
            GoalPoint = Goals[missionIndex].transform;
            Debug.Log($"GoalPoint gesetzt für Mission {missionIndex}: {Goals[missionIndex].name}");
        }
        else
        {
            // Fallback-Logik
            HandleFallbackGoal(missionIndex);
        }
    }

    void HandleFallbackGoal(int missionIndex)
    {
        // Erst versuchen ein Standard-Goal zu finden
        GameObject fallbackGoal = GameObject.Find("Goal");
        
        if (fallbackGoal != null)
        {
            GoalPoint = fallbackGoal.transform;
            Debug.LogWarning($"Goal[{missionIndex}] ist null! Verwende Fallback-Goal: {fallbackGoal.name}");
        }
        else
        {
            // Letzter Ausweg: erstes verfügbare Goal im Array
            for (int i = 0; i < Goals.Length; i++)
            {
                if (Goals[i] != null)
                {
                    GoalPoint = Goals[i].transform;
                    Debug.LogWarning($"Goal[{missionIndex}] und Fallback-Goal nicht gefunden! Verwende Goal[{i}]: {Goals[i].name}");
                    return;
                }
            }
            
            // Wenn gar nichts gefunden wurde
            GoalPoint = null;
            Debug.LogError($"Kein Goal gefunden für Mission {missionIndex}! Bitte Goals im Inspector zuweisen.");
        }
    }

    void RotateTowardsGoal()
    {
        if (GoalPoint != null)
        {
            Vector3 lookdirection = GoalPoint.position - transform.position;
            lookdirection.y = 0; // Nur horizontale Ausrichtung

            // Korrekte Rotation zum Ziel
            if (lookdirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookdirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2f * Time.deltaTime);
            }
        }
    }
}
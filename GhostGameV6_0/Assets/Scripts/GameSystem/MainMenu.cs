using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Canvas canvas1;
    public Canvas canvas2;
    public Canvas MPCanvas3;
    public Canvas SettingsCanvas4;
    public Canvas MPCanvas5;
    public Canvas QuitCanvas6;
    public Canvas LoadCanvas7;
    public Canvas NewGameCanvas8;

    public Canvas LoadingCanvas;

    public bool isMultiplayerReleased = false;

    // ✅ NEU: Scene-Name für das Spiel
    [Header("Game Settings")]
    public string gameSceneName = "GameScene"; // Anpassen an Ihren Scene-Namen
    public int gameSceneIndex = 1; // Falls Sie Scene-Index verwenden

    void Start()
    {
        if (canvas1 != null)
            canvas1.gameObject.SetActive(true);
        if (canvas2 != null)
            canvas2.gameObject.SetActive(false);
        if (MPCanvas3 != null)
            MPCanvas3.gameObject.SetActive(false);
        if (SettingsCanvas4 != null)
            SettingsCanvas4.gameObject.SetActive(false);
        if (MPCanvas5 != null)
        {
            MPCanvas5.gameObject.SetActive(false);
        }
        if (QuitCanvas6 != null)
        {
            QuitCanvas6.gameObject.SetActive(false);
        }
        if (LoadCanvas7 != null)
        {
            LoadCanvas7.gameObject.SetActive(false);
        }
        if (NewGameCanvas8 != null)
        {
            NewGameCanvas8.gameObject.SetActive(false);
        }
        if(LoadingCanvas == null)
        LoadingCanvas = GameObject.Find("LoadingCanvas").GetComponent<Canvas>();

        // ✅ NEU: Überprüfe Savegame-Status beim Start
            CheckSaveGameStatus();
    }

    // ✅ NEU: Überprüfe ob Savegame existiert
    void CheckSaveGameStatus()
    {
        bool hasSaveFile = SaveSystem.HasSaveFile();
        Debug.Log($"Savegame Status: {(hasSaveFile ? "Gefunden" : "Nicht gefunden")}");
        
        // Optional: UI-Elemente entsprechend aktivieren/deaktivieren
        // Hier könnten Sie Load-Button aktivieren/deaktivieren
    }

    public void Singleplayer()
    {
        canvas1.gameObject.SetActive(false);
        canvas2.gameObject.SetActive(true);
        MPCanvas3.gameObject.SetActive(false);
        SettingsCanvas4.gameObject.SetActive(false);
        MPCanvas5.gameObject.SetActive(false);
        QuitCanvas6.gameObject.SetActive(false);
        LoadCanvas7.gameObject.SetActive(false);
        NewGameCanvas8.gameObject.SetActive(false);
    }

    public void Multiplayer()
    {
        if (isMultiplayerReleased == false)
        {
            canvas1.gameObject.SetActive(false);
            canvas2.gameObject.SetActive(false);
            MPCanvas3.gameObject.SetActive(true);
            SettingsCanvas4.gameObject.SetActive(false);
        }
        else if (isMultiplayerReleased == true)
        {
            MPCanvas5.gameObject.SetActive(true);
            canvas1.gameObject.SetActive(false);
            canvas2.gameObject.SetActive(false);
            MPCanvas3.gameObject.SetActive(false);
            SettingsCanvas4.gameObject.SetActive(false);
        }
    }

    public void LoadCanvas()
    {
        LoadCanvas7.gameObject.SetActive(true);
        canvas1.gameObject.SetActive(false);
        canvas2.gameObject.SetActive(false);
        MPCanvas3.gameObject.SetActive(false);
        SettingsCanvas4.gameObject.SetActive(false);
        MPCanvas5.gameObject.SetActive(false);
        QuitCanvas6.gameObject.SetActive(false);
    }

    public void NewGameCanvas()
    {
        NewGameCanvas8.gameObject.SetActive(true);
        canvas1.gameObject.SetActive(false);
        canvas2.gameObject.SetActive(false);
        MPCanvas3.gameObject.SetActive(false);
        SettingsCanvas4.gameObject.SetActive(false);
        MPCanvas5.gameObject.SetActive(false);
        QuitCanvas6.gameObject.SetActive(false);
    }

    public void NewGame()
    {
        // ✅ ERWEITERT: Neues Spiel mit PlayerData System
        Debug.Log("🎮 Neues Spiel wird gestartet...");
        
        // Warnung wenn Savegame existiert
        if (SaveSystem.HasSaveFile())
        {
            Debug.Log("⚠️ Warnung: Bestehendes Savegame wird überschrieben!");
        }
        
        // Flag für neues Spiel setzen
        PlayerPrefs.SetInt("IsNewGame", 1);
        PlayerPrefs.Save();

        // Spiel laden (mit Scene-Index wie original)
        LoadingCanvas.gameObject.SetActive(true);
        DontDestroyOnLoad(LoadingCanvas.gameObject);
        SceneManager.LoadScene(gameSceneIndex);
    }

    public void LoadGameSystem()
    {
        // ✅ ERWEITERT: Spiel laden mit PlayerData System
        if (SaveSystem.HasSaveFile())
        {
            Debug.Log("📂 Gespeichertes Spiel wird geladen...");
            
            // Flag für geladenes Spiel setzen
            PlayerPrefs.SetInt("IsNewGame", 0);
            PlayerPrefs.Save();
            
            // Spiel laden (mit Scene-Index wie original)
              LoadingCanvas.gameObject.SetActive(true);
        DontDestroyOnLoad(LoadingCanvas.gameObject);
            SceneManager.LoadScene(gameSceneIndex);
        }
        else
        {
            Debug.Log("❌ Kein Savegame gefunden!");

            // Optional: Zeige Warnung im UI oder starte neues Spiel
            ShowNoSaveFileMessage();
            
        }
    }

    // ✅ NEU: Zeige Nachricht wenn kein Savegame gefunden
    void ShowNoSaveFileMessage()
    {
        Debug.Log("Kein Savegame gefunden - möchten Sie ein neues Spiel starten?");
        
        // Optional: Hier könnten Sie ein Popup zeigen oder automatisch neues Spiel starten
        // Für jetzt einfach neues Spiel starten:
        // NewGame();
    }

    public void OpenQuitCanvas()
    {
        QuitCanvas6.gameObject.SetActive(true);
        canvas1.gameObject.SetActive(false);
        canvas2.gameObject.SetActive(false);
        MPCanvas3.gameObject.SetActive(false);
        SettingsCanvas4.gameObject.SetActive(false);
        MPCanvas5.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        MPCanvas5.gameObject.SetActive(false);
        canvas1.gameObject.SetActive(true);
        canvas2.gameObject.SetActive(false);
        MPCanvas3.gameObject.SetActive(false);
        SettingsCanvas4.gameObject.SetActive(false);
        QuitCanvas6.gameObject.SetActive(false);
        LoadCanvas7.gameObject.SetActive(false);
        NewGameCanvas8.gameObject.SetActive(false);
    }

    // ✅ NEU: Debug-Methoden (optional)
    [ContextMenu("Check Save Status")]
    void DebugCheckSaveStatus()
    {
        bool hasSave = SaveSystem.HasSaveFile();
        Debug.Log($"Has Save File: {hasSave}");
        
        if (hasSave)
        {
            PlayerData data = SaveSystem.Loadgame();
            if (data != null)
            {
                Debug.Log($"Save Info - Mission: {data.type}");
                Debug.Log($"Keys: {data.KeyName?.Length ?? 0}");
                Debug.Log($"Notes: {data.NoteName?.Length ?? 0}");
            }
        }
    }

    [ContextMenu("Delete Save File")]
    void DebugDeleteSave()
    {
        SaveSystem.DeleteSaveFile();
        Debug.Log("🗑️ Savegame gelöscht!");
    }
}


using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StoryText : MonoBehaviour
{

    
    public string[] Mission0;
    public string[] Mission1;
    public string[] Mission2;

    public string[] Mission3;
    public string[] Mission4;

    public string[] Mission5;

    public string[] Mission6;

    public string[] Mission7;

    public string[] Mission8;

    public string[] Mission9;

    public GameMode gm;


    public int currentTextIndex = 0;
    [HideInInspector] public int index1 = 0;

 
    public TMP_Text TextC; // Universelle TextMeshPro Component
    public GameObject TextObj;
    public GameObject StoryBackground;

    public GameObject PauseCanvas;
    public PauseMenu pauseMenuObj;

    public int index1Lengh;

    public bool SetGameObjActive = false;

    private bool isStoryBackgroundPauseTheGame;

public int STcurrentIndexInArray;


    void Start()
    {
if(gm == null)
gm = GameObject.Find("GameMode").GetComponent<GameMode>();

STcurrentIndexInArray = -1;

        isStoryBackgroundPauseTheGame = false;

        SetGameObjActive = false;
        
        // Array-Initialisierung vor GetComponent
        Mission0 = new string[10];
        Mission1 = new string[3];
        Mission2 = new string[3];
        Mission3 = new string[3];
        Mission4 = new string[2];
        Mission5 = new string[2];
        Mission6 = new string[2];
        Mission7 = new string[2];
        Mission8 = new string[2];
        Mission9 = new string[2];

        if(PauseCanvas != null)
        {
            pauseMenuObj = PauseCanvas.GetComponent<PauseMenu>();
            if (pauseMenuObj == null)
            {
                Debug.LogError("PauseMenu Component nicht gefunden auf " + PauseCanvas.name);
            }
        }
        else
        {
            Debug.LogError("PauseCanvas GameObject ist nicht zugewiesen!");
        }
        
        // TextMeshPro Component finden (funktioniert für UI und 3D)
        if (TextObj != null)
        {
            TextC = TextObj.GetComponent<TMP_Text>();
            if (TextC == null)
            {
                Debug.LogError($"Keine TextMeshPro Component gefunden auf {TextObj.name}! " +
                             "Stellen Sie sicher, dass das GameObject eine TextMeshPro oder TextMeshProUGUI Component hat.");
                return;
            }
            else
            {
                Debug.Log($"TextMeshPro Component erfolgreich gefunden: {TextC.GetType().Name}");
            }
        }
        else
        {
            Debug.LogError("TextObj ist nicht zugewiesen! Ziehen Sie ein GameObject mit TextMeshPro ins Feld.");
            return;
        }

        // Mission-Texte setzen
        Mission0[0] = "Sebastian: Willkommen in unserer Stadt, schön dich wiederzusehen, Jill! Wir haben uns lange nicht gesehen. Was führteuch hierher?";
        Mission0[1] = "JIll: Nach all den Jahren wollten wir nur mal wieder hierher kommen. Das ist meine beste Freundin Lina, sie kannte auch meine Schwester..";
        Mission0[2] = "Sebastian: Hallo Lina! Ach, sie kannte sie auch? Es tut mir so leid, was du damals durchmachen musstest.";
        Mission0[3] = "Jill: ...";
        Mission0[4] = "Sebastian: Kann ich was für euch tun? Euch herumführen?";
        Mission0[5] = "Jill: Ja, gerne! Sag mal, wie kommt man am besten zum Friedhof? Wir wollen meiner Schwester einen Besuch abstatten.";
        Mission0[6] = "Sebastian: In den Wald, um die Zeit? Tut mir leid, niemand geht in den Wald, wenn es dunkel wird!";
        Mission0[7] = "Jill: Wir müssen aber jetzt in den Wald!!!";
        Mission0[8] = "Sebastian: O.k., o.k.! Hier ist ein Talisman. Ich hab ihn selbst gemacht, falls ihr... Ach, nicht so wichtig. Ich begleiteeuch bis zum Wald, ab dann seid ihr auf euch allein gestellt.";
        Mission0[9] = "Jill: Danke, Sebastian...";
       
        Mission1[0] = "Mission 1: Die erste Herausforderung beginnt.";
        Mission1[1] = "Sammle alle Gegenstände ein.";
        Mission1[2] = "Jill: Danke, Sebastian...";

        Mission2[0] = "Mission 2: Tiefer in das Geheimnis.";
        Mission2[1] = "Die Geister werden stärker.";
        Mission2[2] = "Folge den Hinweisen.";

        Mission3[0] = "Mission 3: Das Rätsel löst sich.";
        Mission3[1] = "Du näherst dich der Wahrheit.";
        Mission3[2] = "Aber die Gefahr steigt.";

        Mission4[0] = "Mission 4: Neue Erkenntnisse.";
        Mission4[1] = "Die Vergangenheit enthüllt sich.";

        Mission5[0] = "Mission 5: Der Wendepunkt.";
        Mission5[1] = "Nichts ist wie es scheint.";

        Mission6[0] = "Mission 6: Die Jagd beginnt.";
        Mission6[1] = "Du wirst selbst zum Gejagten.";

        Mission7[0] = "Mission 7: Kampf ums Überleben.";
        Mission7[1] = "Setze alles auf eine Karte.";

        Mission8[0] = "Mission 8: Die finale Konfrontation.";
        Mission8[1] = "Bereite dich auf das Ende vor.";

        Mission9[0] = "Mission 9: Das große Finale.";
        Mission9[1] = "Alles steht auf dem Spiel.";

    }

    void Update()
    {
        if (isStoryBackgroundPauseTheGame == false && pauseMenuObj.isGamePaused == false)
        {
            Time.timeScale = 1; // Spiel läuft normal weiter
        }
        else
        {
            Time.timeScale = 0; // Spiel pausiert
}

        // Story Background Verwaltung basierend auf SetGameObjActive
        if (SetGameObjActive)
        {
           
            if (StoryBackground != null && !StoryBackground.activeInHierarchy)
            {
                StoryBackground.SetActive(true);
                Debug.Log("StoryBackground aktiviert");
            }
        }
        else
        {
            if (StoryBackground != null && StoryBackground.activeInHierarchy)
            {
                StoryBackground.SetActive(false);
                Debug.Log("StoryBackground deaktiviert");
            }
        }

        // Text-Anzeige basierend auf aktueller Mission
        switch (currentTextIndex)
        {
            case 0:
                if (Mission0 != null && index1 < Mission0.Length)
                {
                    
                    TextC.text = Mission0[index1];
                    if (index1 == Mission0.Length - 1)
                    {
                        isStoryBackgroundPauseTheGame = false;
                        SetGameObjActive = false; // Deaktiviere Story Background
                      
                    }
                }
                break;
            case 1:
                if (Mission1 != null && index1 < Mission1.Length)
                {
                   
                    TextC.text = Mission1[index1];
                    if (index1 == Mission1.Length - 1)
                    {
                        isStoryBackgroundPauseTheGame = false;
                        SetGameObjActive = false; // Deaktiviere Story Background
                    
                    }
                }
                break;
            case 2:
                if (Mission2 != null && index1 < Mission2.Length)
                {
                
                    TextC.text = Mission2[index1];
                    if (index1 == Mission2.Length - 1)
                    {
                        isStoryBackgroundPauseTheGame = false;
                        SetGameObjActive = false; // Deaktiviere Story Background
                   
                    }
                }
                break;
            case 3:
                if (Mission3 != null && index1 < Mission3.Length)
                {
                    
                    TextC.text = Mission3[index1];
                    if (index1 == Mission3.Length - 1)
                    {
                   isStoryBackgroundPauseTheGame = false;
                        if (StoryBackground != null)
                            StoryBackground.SetActive(false);
               
                    
             
                    }
                }
                break;
            case 4:
                if (Mission4 != null && index1 < Mission4.Length)
                {
              
                    TextC.text = Mission4[index1];
                    if (index1 == Mission4.Length - 1)
                    {
                        isStoryBackgroundPauseTheGame = false;
                        if (StoryBackground != null)
                            StoryBackground.SetActive(false);
             
                
           
                    }
                }
                break;
            case 5:
                if (Mission5 != null && index1 < Mission5.Length)
                {
                 
                    TextC.text = Mission5[index1];
                    if (index1 == Mission5.Length - 1)
                    {
                            isStoryBackgroundPauseTheGame = false;
                  
                        if (StoryBackground != null)
                            StoryBackground.SetActive(false);
                
           
                 
                   }
                }
                break;
            case 6:
                if (Mission6 != null && index1 < Mission6.Length)
                {
                 
                    TextC.text = Mission6[index1];
                    if (index1 == Mission6.Length - 1)
                        
                    { 
                        isStoryBackgroundPauseTheGame = false;
                        

                        if (StoryBackground != null)
                            StoryBackground.SetActive(false);
          

              
                    }
                }
                break;
            case 7:
                if (Mission7 != null && index1 < Mission7.Length)
                {
              
                    TextC.text = Mission7[index1];
                    if (index1 == Mission7.Length - 1)
                    {
                        isStoryBackgroundPauseTheGame = false;
                        if (StoryBackground != null)
                            StoryBackground.SetActive(false);
 
                    
             
                  
                    }
                }
                break;
            case 8:
                if (Mission8 != null && index1 < Mission8.Length)
                {
                 
                    TextC.text = Mission8[index1];
                    if (index1 == Mission8.Length - 1)
                    {
                        isStoryBackgroundPauseTheGame = false;
                        if (StoryBackground != null)
                            StoryBackground.SetActive(false);
                       
                 
                
                    }
                }
                break;
            case 9:
                if (Mission9 != null && index1 < Mission9.Length)
                {
                 
                    TextC.text = Mission9[index1];
                    if (index1 == Mission9.Length - 1)
                    {
                        isStoryBackgroundPauseTheGame = false;
                        if (StoryBackground != null)
                            StoryBackground.SetActive(false);
                        if (gm.currentMIssion == GameMode.MissionType.Mission9)
            {
                gm.currentMIssion = GameMode.MissionType.Mission10;
            }
                  
                    }
                }
                break;
        }
    }

    public void NewText()
    {
        index1++;
    }

    // Öffentliche Methode zum Starten einer bestimmten Mission Story
    public void StartMissionStory()
    {
        STcurrentIndexInArray++;
        currentTextIndex = STcurrentIndexInArray;
     isStoryBackgroundPauseTheGame = true;
        StoryBackground.SetActive(true);
        index1 = 0;
        SetGameObjActive = true;
    // Spiel pausieren
        
        Debug.Log($"Mission Story gestartet - StoryBackground wird aktiviert");
    }

    // Neue Methode: Startet die nächste Mission Story automatisch
  

    
}

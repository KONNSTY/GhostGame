using TMPro;
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
    public string[] Mission10;

    private string[][] missions;

    public GameMode gm;

    public int currentTextIndex = 0;
    public int index1 = 0;

    public TMP_Text TextC; // Universelle TextMeshPro Component
    public GameObject TextObj;
    public GameObject StoryBackground;

    public GameObject PauseCanvas;
    public PauseMenu pauseMenuObj;

    public int index1Lengh;

    public bool SetGameObjActive = false;

    private bool isStoryBackgroundPauseTheGame;

    private bool isTriggered = false;

    public int STcurrentIndexInArray;

    private string[] currentMissionArray;

    void Start()
    {
        if (gm == null)
            gm = GameObject.Find("GameMode")?.GetComponent<GameMode>();

        STcurrentIndexInArray = -1;

        isStoryBackgroundPauseTheGame = false;
        SetGameObjActive = false;

        // Mission-Arrays initialisieren
        // Mission0 bleibt LEER gemäß Anforderung
        Mission0 = new string[20];

        Mission1 = new string[12];
        Mission2 = new string[1];
        Mission3 = new string[2];
        Mission4 = new string[1];
        Mission5 = new string[4];
        Mission6 = new string[4];
        Mission7 = new string[5];
        Mission8 = new string[2];
   


        if (PauseCanvas != null)
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

        Mission0[0] = "When the two girls arrive by ferry in the small town, they are greeted by Sebastian—a strange man, a family friend.";
        Mission0[1] = "He always wears this odd suit. He welcomes them but warns them not to enter the forest.";
        Mission0[2] = "Ever since Jill's sister disappeared, nobody goes there anymore.";
        Mission0[3] = "People say strange things happen. The police closed off the area, claiming it's unsafe.";
        Mission0[4] = "The girls tell him they plan to go into the forest, but he insists it's a bad idea.";
        Mission0[5] = "He can't stop them, so he gives them flashlights he built himself—unused prototypes.";
        Mission0[6] = "He says he is fascinated by the supernatural, and rumors say the forest is haunted.";
        Mission0[7] = "The flashlights, supposedly, can repel spirits.";
        Mission0[8] = "Jill and Lina wander through town before deciding to head into the forest.";
        Mission0[9] = "Sebastian: \"Welcome back to our town, Jill! It's been a long time. What brings you here?\"";
        Mission0[10] = "Jill: \"After all these years, we just wanted to come back. This is my best friend Lina—she knew my sister too.\"";
        Mission0[11] = "Sebastian: \"Hello, Lina! She knew her as well? I'm so sorry for what you went through.\"";
        Mission0[12] = "Jill: \"…\"";
        Mission0[13] = "Sebastian: \"Can I help you? Want a tour?\"";
        Mission0[14] = "Jill: \"Sure. Tell me… how do we get to the cemetery? We want to visit my sister.\"";
        Mission0[15] = "Sebastian: \"Into the forest? At this hour? No one goes into the woods once it gets dark!\"";
        Mission0[16] = "Jill: \"But we need to go now!\"";
        Mission0[17] = "Sebastian: \"Alright, alright! Here, take this talisman. I made it myself, just in case… well, never mind.";
        Mission0[18] = "I'll walk you to the forest entrance. From there, you're on your own.\"";
        Mission0[19] = "Jill: \"Thank you, Sebastian…\"";

        // Mission-Texte setzen (Mission0 bleibt leer)
        Mission1[0] = "Inside the woods, the girls turn on their flashlights.";
        Mission1[1] = "As they walk deeper, they suddenly hear ghostly whispers.";
        Mission1[2] = "Spirits appear out of nowhere. The girls must fight them off.";
        Mission1[3] = "After defeating the spirits, they try to leave—but some force stops them. They are trapped.";
        Mission1[4] = "They venture further and find a note. Jill thinks it might be from her sister. It reads:";
        Mission1[5] = "'I'm so alone. Ever since I came here, I haven't been able to leave.";
        Mission1[6] = "The forest won't let me. It won't let me.'";
        Mission1[7] = "As they continue, they suddenly see Jill's sister.";
        Mission1[8] = "She whispers:";
        Mission1[9] = "'Why didn't you look for me?'";
        Mission1[10] = "Then she vanishes.";
        Mission1[11] = "The path is blocked by the same mysterious force. The girls find a strange key and manage to pass through.";

        Mission2[0] = "We need to go deeper into the forest.";

        Mission3[0] = "This path leads to a graveyard.";
        Mission3[1] = "Lina: \"Where is the key to the grave? Let's keep searching for it.\"";

        Mission4[0] = "Let's find the key and go to the grave.";

        Mission5[0] = "Something feels wrong. They find graves with their own names engraved on them.";
        Mission5[1] = "Beside Jill's grave lies a supposed farewell letter—from Jill—claiming she couldn't bear the guilt over her sister anymore.";
        Mission5[2] = "They look for a shovel and dig up Jill's grave.";
        Mission5[3] = "It is empty.";

        Mission6[0] = "The next area is engulfed in light. The girls are pulled in.";
        Mission6[1] = "Suddenly they're in Jill's apartment—her sister standing there, acting completely normal.";
        Mission6[2] = "But when Jill and Lina try to confront her about the illusion, she avoids the questions. They play along for now.";
        Mission6[3] = "A fight breaks out between Jill and Lina. Lina storms into Jill's room.";

        Mission7[0] = "Why is it so quiet here?";
        Mission7[1] = "When Jill follows, Lina can suddenly no longer see her. She's on the phone, talking about Jill's funeral. The vision abruptly ends.";

        Mission7[0] = "They reach the final place: the grave of Jill's sister. Her sister stands before it and runs toward Jill.";
        Mission7[1] = "Sister: 'You finally found me. Now we can go.'";
        Mission7[2] = "Jill: 'We can't go. You're dead.'";
        Mission7[3] = "Her sister: 'Come on, Jill… let's go. Let's be together forever.'";
        Mission7[4] = "She runs ahead.";

        Mission8[0] = "The girls stand before the town gate. Lina? Where is Lina? Jill realizes no one recognizes her. An interview with Lina appears.";
        Mission8[1] = "'Jill, we're finally together again.' A bright light appears before them, and they walk into it.";

        // Mapping erstellen — Mission0 ist bewusst leer
        missions = new string[][]
        {
            Mission0, Mission1, Mission2, Mission3, Mission4,
            Mission5, Mission6, Mission7, Mission8, Mission9, Mission10
        };

        Debug.Log("StoryText initialisiert (Mission0 leer).");
    }

    void Update()
    {
        Debug.Log(currentTextIndex + " StoryTextindex" + (int)gm.currentMIssion + " GameModeindex");

        if (pauseMenuObj != null)
        {
            if (isStoryBackgroundPauseTheGame == false && pauseMenuObj.isGamePaused == false)
            {
                Time.timeScale = 1; // Spiel läuft normal weiter
            }
            else
            {
                Time.timeScale = 0; // Spiel pausiert
            }
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

        // Text-Anzeige basierend auf aktueller Mission (verwendet gm.currentMIssion)
        if (TextC == null) return;
        if (!isTriggered) return;

        int missionIndex;
        if (gm != null)
            missionIndex = (int)gm.currentMIssion -1;
        else
            missionIndex = currentTextIndex;

        // sichere Bounds- und Null-Checks über das missions-Array
        if (missions == null || missionIndex < 0 || missionIndex >= missions.Length)
        {
            EndStory();
            return;
        }

        currentMissionArray = missions[missionIndex];
        if (currentMissionArray == null || currentMissionArray.Length == 0)
        {
            EndStory();
            return;
        }

        if (index1 < currentMissionArray.Length)
        {
            TextC.text = currentMissionArray[index1];
        }
        else
        {
            EndStory();
        }
    }

    private void EndStory()
    {
        isStoryBackgroundPauseTheGame = false;
        isTriggered = false;
        SetGameObjActive = false;
        if (StoryBackground != null)
            StoryBackground.SetActive(false);

        // Automatische Mission-Weiterschaltung wenn am Ende einer Mission
        if (gm != null && index1 >= currentMissionArray.Length)
        {
            // Prüfe ob wir bei Mission9 sind (Index 8 im Array, aber Mission9 im Enum)
            if (gm.currentMIssion == GameMode.MissionType.Mission9)
            {
                gm.currentMIssion = GameMode.MissionType.Mission10;
                Debug.Log("Mission automatisch von Mission9 zu Mission10 gewechselt!");
            }
        }

        Debug.Log("Story beendet.");
    }

    public void NewText()
    {
        index1++;
    }

    // Öffentliche Methode zum Starten einer bestimmten Mission Story
    public void StartMissionStory()
    {
        if (gm == null)
            gm = GameObject.Find("GameMode")?.GetComponent<GameMode>();

        isTriggered = true;

        if (gm != null)
        {
            currentTextIndex = (int)gm.currentMIssion -1;

                    }

        else
            currentTextIndex = 0;

        STcurrentIndexInArray = currentTextIndex;

        isStoryBackgroundPauseTheGame = true;
        if (StoryBackground != null)
            StoryBackground.SetActive(true);
        index1 = 0;
        SetGameObjActive = true;

        Debug.Log($"Mission Story gestartet - MissionIndex={STcurrentIndexInArray}");
    }
}

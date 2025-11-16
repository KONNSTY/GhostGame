using System;
using TMPro;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    public int aiSpawners;

    public GameObject DirectionalLight;
    private Animator dirLightAnimator;

    public GameObject Player;
    private PlayerInventory playerInventory;

    //Depending on Load Game ->>>>

    private bool isMissionNotLoadAndNight = false;

public bool BossDefeated = false;
 

    public string Mission0;
    public string Mission1;
    public string Mission2;
    public string Mission3;
    public string Mission4;
    public string Mission5;
    public string Mission6;
    public string Mission7;
    public string Mission8;
    public string Mission9;
    public string Mission10;
    public string Mission11;
    public string Mission12;

    public NavMeshSurface navMeshSurface;

    public GameObject WeaponInLightObj;
    private WeaponInLight weaponInLight;

    public MissionType currentMIssion;
    
    public GameObject[] GoalArray = new GameObject[10];


    public AiSpawner LastSpawner;


    public TextMeshProUGUI missionText;

    public GameObject GameUi;
    private StoryText storyText;
   

   
    public NPcFollowScript[] NPCFollowObj = new NPcFollowScript[3];

    public GameObject[] Portals = new GameObject[6];
    public interblock[] interblocksObj = new interblock[6];

    // ✅ HINZUGEFÜGT: Performance-Optimierun
    private MissionType lastProcessedMission;

    [HideInInspector] public bool shouldAiSpawnerAllDeactivate;

    public GameObject FirstCutSceneCanvasObj;

    public enum MissionType
    {
        Mission0, Mission1, Mission2, Mission3, Mission4, Mission5,
        Mission6, Mission7, Mission8, Mission9, Mission10, Mission11, Mission12,
    }

    void Awake()
    {
        // Null-Prüfung für StoryText Component
        if (GameUi != null)
        {
            storyText = GameUi.GetComponent<StoryText>();
            if (storyText == null)
            {
                Debug.LogError("StoryText Component nicht gefunden auf GameUi GameObject!");
            }
        }
    }

    void Start()
    {
        dirLightAnimator = DirectionalLight.GetComponent<Animator>();
        weaponInLight = WeaponInLightObj.GetComponent<WeaponInLight>();

        playerInventory = Player.GetComponent<PlayerInventory>();

        for (int i = 0; i < Portals.Length; i++)
        {
            if (Portals[i] == null)
                Portals[i] = GameObject.Find("Portal blue" + i);
        }
         
         for (int i = 0; i < interblocksObj.Length; i++)
         {
        interblocksObj[i] = Portals[i].GetComponent<interblock>();
         }

        shouldAiSpawnerAllDeactivate = false;
    }

    void Update()
    {
        // ✅ HINZUGEFÜGT: Performance-Optimierung - Goals nur einmal pro Mission aktivieren
        if (currentMIssion != lastProcessedMission)
        {

            lastProcessedMission = currentMIssion;
        }

        if(currentMIssion != MissionType.Mission0)
        {
            GoalArray[0].SetActive(false);
        }else
        {
            GoalArray[0].SetActive(true);
        }







        switch (currentMIssion)
        {
            case MissionType.Mission0:

                missionText.text = Mission0;
                FirstCutSceneCanvasObj.SetActive(true);
                weaponInLight.isUnableToUsWeapon = true;
                if (NPCFollowObj[2].isSisterFollowing == true)
                    NPCFollowObj[2].isSisterFollowing = false;
                // ✅ KORRIGIERT: Null-Check hinzugefügt
                if (NPCFollowObj[0] != null)
                    NPCFollowObj[0].isSebastianFollowing = false;
                GoalArray[0].SetActive(true);
                isMissionNotLoadAndNight = false;

             
                break;

            case MissionType.Mission1:
                missionText.text = Mission1;
                FirstCutSceneCanvasObj.SetActive(false);
                aiSpawners = 1; // ✅ FIX: Nur 1 Geist für Anfänger-Mission
                // ✅ KORRIGIERT: Besserer Null-Check
                NPCFollowObj[0].isSebastianFollowing = true;
         GoalArray[1].SetActive(true);
               


                isMissionNotLoadAndNight = false;

                break;

            case MissionType.Mission2:
                missionText.text = Mission2;
                FirstCutSceneCanvasObj.SetActive(false);
                  aiSpawners = 1; // ✅ FIX: Nur 1 Geist für Anfänger-Mission
                // ✅ HINZUGEFÜGT: Sebastian stoppt zu folgen
                GoalArray[2].SetActive(true);
                NPCFollowObj[0].isSebastianFollowing = false;
                playerInventory.RemoveKey("Key1", 1);

                // ✅ HINZUGEFÜGT: Waffe aktivieren
                weaponInLight.isUnableToUsWeapon = false;
                dirLightAnimator.SetBool("Night", true);
                dirLightAnimator.SetBool("Day", false);
                if (playerInventory.inventoryListKey.Contains("Key1" + 1))
                    playerInventory.RemoveKey("Key1", 1);
              
                // ✅ FIX: Aktiviere Portale in Mission 2
                for (int i = 0; i < Portals.Length; i++)
                {
                    if (Portals[i] != null)
                        Portals[i].SetActive(true);
                }
       
                isMissionNotLoadAndNight = true;


                break;

            case MissionType.Mission3:
                missionText.text = Mission3;
                FirstCutSceneCanvasObj.SetActive(false);

            aiSpawners = 3;

                    GoalArray[3].SetActive(true);

                weaponInLight.isUnableToUsWeapon = false;

                if (isMissionNotLoadAndNight == false)
                {
                    dirLightAnimator.SetBool("Night", true);
                    dirLightAnimator.SetBool("Day", false);
                    isMissionNotLoadAndNight = true;
                }

               

                break;

            case MissionType.Mission4:
                missionText.text = Mission4;
                FirstCutSceneCanvasObj.SetActive(false);
                GoalArray[4].SetActive(true);
           aiSpawners = 3;
                weaponInLight.isUnableToUsWeapon = false;


                if (isMissionNotLoadAndNight == false)
                {
                    dirLightAnimator.SetBool("Night", true);
                    dirLightAnimator.SetBool("Day", false);
                    isMissionNotLoadAndNight = true;
                }

             
             

                break;

            case MissionType.Mission5:
                missionText.text = Mission5;
                FirstCutSceneCanvasObj.SetActive(false);
   aiSpawners = 3;
                GoalArray[5].SetActive(true);
                weaponInLight.isUnableToUsWeapon = false;

                if (isMissionNotLoadAndNight == false)
                {
                    dirLightAnimator.SetBool("Night", true);
                    dirLightAnimator.SetBool("Day", false);
                    isMissionNotLoadAndNight = true;
                }

           

                break;

            case MissionType.Mission6:
                missionText.text = Mission6;
                FirstCutSceneCanvasObj.SetActive(false);
                GoalArray[6].SetActive(true);
                weaponInLight.isUnableToUsWeapon = false;
   aiSpawners = 3;
                if (isMissionNotLoadAndNight == false)
                {
                    dirLightAnimator.SetBool("Night", true);
                    dirLightAnimator.SetBool("Day", false);
                    isMissionNotLoadAndNight = true;
                }


                break;

            case MissionType.Mission7:
                missionText.text = Mission7;
                FirstCutSceneCanvasObj.SetActive(false);

                GoalArray[7].SetActive(true);
                weaponInLight.isUnableToUsWeapon = false;
   aiSpawners = 3;
                if (isMissionNotLoadAndNight == false)
                {
                    dirLightAnimator.SetBool("Night", true);
                    dirLightAnimator.SetBool("Day", false);
                    isMissionNotLoadAndNight = true;
                }
         

                break;

            case MissionType.Mission8:
                missionText.text = Mission8;
                FirstCutSceneCanvasObj.SetActive(false);
                aiSpawners = 1; // ✅ BOSS: Nur 1 Boss spawnen
                weaponInLight.isUnableToUsWeapon = false;


                if(BossDefeated == true)
                {

                   if (GoalArray[8] != null)
                {


                    GoalArray[8].SetActive(true);
                }
                else
                {
                    GoalArray[8] = GameObject.Find("Goal8");
                    GoalArray[8].SetActive(true);
                }
                }

                if (isMissionNotLoadAndNight == false)
                {
                    dirLightAnimator.SetBool("Night", true);
                    dirLightAnimator.SetBool("Day", false);
                    isMissionNotLoadAndNight = true;
                }

                // ✅ BOSS: Nur 1 Boss pro Spawn (aiSpawners bereits auf 1 gesetzt)

                break;

            case MissionType.Mission9:
                missionText.text = Mission9;
                FirstCutSceneCanvasObj.SetActive(false);

                shouldAiSpawnerAllDeactivate = true;
                weaponInLight.isUnableToUsWeapon = true;
                dirLightAnimator.SetBool("Day", true);
                dirLightAnimator.SetBool("Night", false);
                NPCFollowObj[2].isSisterFollowing = true;
                GoalArray[9].SetActive(true);
   aiSpawners = 3;
                isMissionNotLoadAndNight = false;
              

                  for (int i = 0; i < Portals.Length; i++)
                {
                    if (Portals[i] != null)
                        Portals[i].SetActive(false);
                }

                break;

            case MissionType.Mission10:
                missionText.text = Mission10;
                FirstCutSceneCanvasObj.SetActive(false);
                GoalArray[9].SetActive(false);
                SceneManager.LoadScene(3);
                break;
            case MissionType.Mission11:
                missionText.text = Mission11;
                break;
            case MissionType.Mission12:
                missionText.text = Mission12;
                break;
        }
        
        
    }

    // ✅ HINZUGEFÜGT: Methode zum Deaktivieren aller Goals
}
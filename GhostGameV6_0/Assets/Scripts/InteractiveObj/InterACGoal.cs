using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class InterACGoal : MonoBehaviour
{
    public GameObject goaleffect;



    public GameMode.MissionType missiontype;
    public GameMode gm;

    public GameObject gameUI;
    public StoryText storyText;

    public GameObject[] GoalArray = new GameObject[10];




    // StoryText and GameMode reference



    void Start()
    {
        if (gm == null)
        {
            GameObject gmObj = GameObject.Find("GameMode");
            gm = gmObj.GetComponent<GameMode>();
        }

        if (gameUI == null)
        {
            gameUI = GameObject.Find("GameUI");
        }

        if (gameUI != null)
            storyText = gameUI.GetComponent<StoryText>();

        if (storyText != null && gameObject.name != "Goal")
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            gm.currentMIssion = missiontype;


            SaveSystem.SavePlayer(gm, other.gameObject);
            storyText.StartMissionStory();
            gameObject.SetActive(false);

           




        }
    }
}
    



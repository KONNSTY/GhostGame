using UnityEngine;

public class MultiplayerUI : MonoBehaviour
{
    [SerializeField]
    private GameObject publicPrivatePanel;
    [SerializeField]
    private GameObject loadingPanel;
    [SerializeField]
    private GameObject waitngRoomPanel;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowLoading();
        MultiplayerGameManager.Singleton.PlayerJoinedLobby += Game_PlayerJoinedLobby;
        MultiplayerGameManager.Singleton.OnRoomCreated += Player_OnRoomCreated;
        MultiplayerGameManager.Singleton.OnRoomJoined += Player_OnRoomCreated;
    }

    private void Player_OnRoomCreated()
    {
        showwaitngRoomPanel();
    }

    private void Game_PlayerJoinedLobby()
    {
        showPublicPrivatePanel();
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void TurnOffAllPanels()
    {
        publicPrivatePanel.SetActive(false);
        loadingPanel.SetActive(false);
        waitngRoomPanel.SetActive(false);
    }

    public void ShowLoading()
    {
        TurnOffAllPanels();
        loadingPanel.SetActive(true);
    }

    public void showPublicPrivatePanel()
    {
        TurnOffAllPanels();
        publicPrivatePanel.SetActive(true);
    }



    public void FindPublicRoom()
    {
        MultiplayerGameManager.Singleton.FindPUblicRoom();
    }

    public void showwaitngRoomPanel()
    {
        TurnOffAllPanels();
        waitngRoomPanel.SetActive(true);
    }
}

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
        if (MultiplayerGameManager.Singleton == null)
        {
            Debug.LogError("MultiplayerGameManager is missing.");
            return;
        }

        MultiplayerGameManager.Singleton.PlayerJoinedLobby += Game_PlayerJoinedLobby;
        MultiplayerGameManager.Singleton.OnRoomCreated += Player_OnRoomCreated;
        MultiplayerGameManager.Singleton.OnRoomJoined += Player_OnRoomCreated;
    }

    void OnDestroy()
    {
        if (MultiplayerGameManager.Singleton == null)
            return;

        MultiplayerGameManager.Singleton.PlayerJoinedLobby -= Game_PlayerJoinedLobby;
        MultiplayerGameManager.Singleton.OnRoomCreated -= Player_OnRoomCreated;
        MultiplayerGameManager.Singleton.OnRoomJoined -= Player_OnRoomCreated;
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
        if (publicPrivatePanel != null)
            publicPrivatePanel.SetActive(false);
        if (loadingPanel != null)
            loadingPanel.SetActive(false);
        if (waitngRoomPanel != null)
            waitngRoomPanel.SetActive(false);
    }

    public void ShowLoading()
    {
        TurnOffAllPanels();
        if (loadingPanel != null)
            loadingPanel.SetActive(true);
    }

    public void showPublicPrivatePanel()
    {
        TurnOffAllPanels();
        if (publicPrivatePanel != null)
            publicPrivatePanel.SetActive(true);
    }



    public void FindPublicRoom()
    {
        if (MultiplayerGameManager.Singleton != null)
            MultiplayerGameManager.Singleton.FindPUblicRoom();
    }

    public void showwaitngRoomPanel()
    {
        TurnOffAllPanels();
        if (waitngRoomPanel != null)
            waitngRoomPanel.SetActive(true);
    }
}

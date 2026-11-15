using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class MultiplayerGameManager : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    public static MultiplayerGameManager Singleton;

    public event Action PlayerJoinedLobby;

    public event Action OnRoomCreated;

    public event Action OnRoomJoined;
    private List<RoomInfo> roomList = new List<RoomInfo>();

    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
            PhotonNetwork.ConnectUsingSettings();
        }
        else if (Singleton != this)
        {
            Destroy(gameObject); // Verhindert Duplikate
            return;
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();

    }

    public override void OnJoinedLobby()
    {
        Debug.Log("PlayerJoined Lobby");
        PlayerJoinedLobby?.Invoke(); // Event aufrufen
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (roomList == null)
            return;

        foreach (RoomInfo info in roomList)
        {
            if (info == null || string.IsNullOrEmpty(info.Name))
                continue;

            int index = this.roomList.FindIndex(room => room.Name == info.Name);
            if (info.RemovedFromList)
            {
                if (index >= 0)
                    this.roomList.RemoveAt(index);
                continue;
            }

            if (index >= 0)
                this.roomList[index] = info;
            else
                this.roomList.Add(info);
        }
    }

    public void FindPUblicRoom()
    {
        bool found = false;
        RoomInfo foundedRoom = null;

        foreach (RoomInfo room in roomList)
        {
            if (IsJoinablePublicRoom(room))
            {
                found = true;
                foundedRoom = room;
                break;
            }
        }

        if (found == true)
        {
            JoinPublicRoom(foundedRoom);
            Debug.Log("Joining room: " + foundedRoom.Name);
        }
        else
        {
            CreatePublicRoom();
        }
    }

    public void CreatePublicRoom()
    {
        string roomCode = GenerateRoomCode();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2; // Set the maximum number of players in the room
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
        {
            {"isPrivate", false }
        };
        roomOptions.CustomRoomPropertiesForLobby = new[] { "isPrivate" };
        PhotonNetwork.CreateRoom(roomCode, roomOptions);

    }

    public void JoinPublicRoom(RoomInfo room)
    {
        PhotonNetwork.JoinRoom(room.Name);
    }

    public string GenerateRoomName()
    {
        string roomName = "Room" + UnityEngine.Random.Range(10000, 99999);
        return roomName;
    }

    private string GenerateRoomCode()
    {
        string roomCode = "Room" + UnityEngine.Random.Range(10000, 99999);
        return roomCode;
    }

    static bool IsJoinablePublicRoom(RoomInfo room)
    {
        if (room == null || room.RemovedFromList || !room.IsOpen || !room.IsVisible)
            return false;
        if (room.PlayerCount >= room.MaxPlayers)
            return false;
        if (room.CustomProperties == null || !room.CustomProperties.ContainsKey("isPrivate"))
            return false;

        object value = room.CustomProperties["isPrivate"];
        return value is bool isPrivate && !isPrivate;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        OnRoomJoined?.Invoke();// Event aufrufen, wenn der Raum erfolgreich betreten wurde
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("New player entered room");
    }
    
    public override void OnCreatedRoom()
    {
        Debug.Log("Room created successfully");
        OnRoomCreated?.Invoke(); // Event aufrufen, wenn der Raum erfolgreich erstellt wurde
    }
}

 

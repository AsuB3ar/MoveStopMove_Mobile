using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class NetworkManager : MonoBehaviourPunCallbacks,IOnEventCallback
{
    public event Action<Player, bool> _OnPlayerStatusRoomChange;
    public event Action _OnDisconnectServer;
    public event Action _OnLeftRoom;
    public event Action _OnJoinedRoom;
    public event Action _OnConnectedToMaster;
    public event Action _OnJoinedLobby;
    
    public event Action<int> _OnEnemyCountChange;
    public event Action<int[], int[], string[]> _OnPlayerAddPoint;

    private static NetworkManager inst = null;
    public static NetworkManager Inst => inst;
    public bool IsMasterClient => PhotonNetwork.IsMasterClient;
    public Player LocalPlayer => PhotonNetwork.LocalPlayer;
    RaiseEventOptions RaiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
    public enum EVENT
    {
        PLAYER_ADD_POINT = 0,
        ENEMY_COUNT_CHANGE = 1,
    }
    private void Awake()
    {
        if(inst != null)
        {
            Destroy(gameObject);
        }
        else
        {
            inst = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    public void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log($"<color=green>NETWORK</color>: Connecting To Server");
    }

    public override void OnConnectedToMaster()
    {
        _OnConnectedToMaster?.Invoke();
        Debug.Log($"<color=green>NETWORK</color>: Connected To Master");
    }

    public override void OnJoinedLobby()
    {
        _OnJoinedLobby?.Invoke();
        Debug.Log($"<color=green>NETWORK</color>: Joined Lobby");
    }

    public void CreateRoom(string name, string nickname ="")
    {
        PhotonNetwork.NickName = nickname;
        PhotonNetwork.CreateRoom(name);
        Debug.Log($"<color=green>NETWORK</color>: Create Room");
    }

    public void JoinRoom(string name, string nickname = "")
    {
        PhotonNetwork.NickName = nickname;
        PhotonNetwork.JoinRoom(name);
        Debug.Log($"<color=green>NETWORK</color>: Join Room");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void DisconnectMaster()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnJoinedRoom()
    {       
        _OnJoinedRoom?.Invoke();
        Debug.Log($"<color=green>NETWORK</color>: Joined Room");
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        _OnLeftRoom?.Invoke();
        for (int i = 0; i < PhotonPropertyGameObject.AllObjects.Count; i++)
        {
            if (PhotonPropertyGameObject.AllObjects[i].IsMine)
                Destroy(PhotonPropertyGameObject.AllObjects[i].gameObject);
        }
        Debug.Log($"<color=green>NETWORK</color>: Leaved Room");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        _OnDisconnectServer?.Invoke();
        Debug.Log($"<color=green>NETWORK</color>: Disconnected Server");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for(int i = 0; i < roomList.Count; i++)
        {
            Debug.Log($"<color=green>NETWORK - ROOM</color>:{roomList[0].Name}");
        }
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        _OnPlayerStatusRoomChange?.Invoke(newPlayer, true);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        _OnPlayerStatusRoomChange?.Invoke(otherPlayer, false);
    }
    public GameObject Instantiate(string name, Vector3 position = default, Quaternion rotation = default)
    {
        return PhotonNetwork.Instantiate(name, position, rotation);
    }
    public void Destroy(GameObject gameObject)
    {
        PhotonNetwork.Destroy(gameObject);
    }
    public void ClearEvent()
    {
        _OnConnectedToMaster = null;
        _OnJoinedLobby = null;
        _OnJoinedRoom = null;
        _OnDisconnectServer = null;
        _OnLeftRoom = null;
    }  
    public void OnEvent(EventData photonEvent)
    {
        EVENT eventCode = (EVENT)photonEvent.Code;
        object[] data;

        switch (eventCode)
        {
            case EVENT.ENEMY_COUNT_CHANGE:
                data = (object[])photonEvent.CustomData;
                _OnEnemyCountChange?.Invoke((int)data[0]);
                Debug.Log($"<color=yellow>EVENT</color> - Global: {eventCode}");
                break;
            case EVENT.PLAYER_ADD_POINT:
                data = (object[])photonEvent.CustomData;
                int[] id = new int[data.Length / 3];
                int[] value = new int[data.Length / 3];
                string[] name = new string[data.Length / 3];
                for(int i = 0; i < id.Length; i++)
                {
                    id[i] = (int)data[i * 3];
                    value[i] = (int)data[i * 3 + 1];
                    name[i] = (string)data[i * 3 + 2];
                }
                _OnPlayerAddPoint?.Invoke(id, value, name);
                Debug.Log($"<color=yellow>EVENT</color> - Global: {eventCode}");
                break;
        }
        
    }
    public void RaiseEvent(EVENT eventCode, object[] data)
    {
        PhotonNetwork.RaiseEvent((byte)eventCode
            , data 
            , RaiseEventOptions
            , SendOptions.SendUnreliable);
    }
}

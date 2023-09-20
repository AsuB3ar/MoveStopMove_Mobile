using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Utilitys;
using System;
using MoveStopMove.Manager;
using Photon.Realtime;

public class PhotonPrefabManager : MonoBehaviourPun, ISyncState
{
    [SerializeField]
    PrefabManager prefabManager;
    List<KeyValuePair<int, int>> pools;
    ISyncState.STATE state = ISyncState.STATE.ON_INIT;
    public ISyncState.STATE State => state;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            state = ISyncState.STATE.READY;          
        }
        if(PhotonNetwork.IsMasterClient)
            NetworkManager.Inst._OnPlayerStatusRoomChange += OnPlayerEnterRoom;
    }
    public void SetSerializeData(ref List<KeyValuePair<int, int>> pools)
    {
        this.pools = pools;             
    }

    [PunRPC]
    protected void RPC_Init_Data(object[] data)
    {
        int count = (int)data[0];
        pools.Clear();
        for (int i = 0; i < count; i++)
        {
            pools.Add(new KeyValuePair<int, int>((int)data[i * 2 + 1], (int)data[i * 2 + 2]));
        }
        prefabManager.InitPhotonData();
        Debug.Log("ON PREFAB MANAGER SERIALIZE DATA");
        state = ISyncState.STATE.READY;
    }

    public void UpdatePhotonData()
    {
        object[] data = GetObjectData();
        photonView.RPC(nameof(RPC_Init_Data), RpcTarget.Others, data as object);
    }

    private void OnPlayerEnterRoom(Player player, bool value)
    {      
        if (value)
        {
            object[] data = GetObjectData();
            photonView.RPC(nameof(RPC_Init_Data), player, data as object);
        }
    }

    private object[] GetObjectData()
    {
        object[] data = new object[pools.Count * 2 + 1];
        data[0] = pools.Count;
        for (int i = 0; i < pools.Count; i++)
        {
            data[i * 2 + 1] = pools[i].Key;
            data[i * 2 + 2] = pools[i].Value;
        }

        return data;
    }

    private void OnDestroy()
    {
        if(PhotonNetwork.IsMasterClient)
            NetworkManager.Inst._OnPlayerStatusRoomChange -= OnPlayerEnterRoom;
    }
}

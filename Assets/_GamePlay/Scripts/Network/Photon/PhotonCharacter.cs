using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonCharacter : MonoBehaviourPun
{
    // Start is called before the first frame update
    public enum EVENT
    {
        TAKE_DAMAGE = 0,
        LEVEL_UP = 1,
        REVIVE = 2,
    }
    public event Action<int[]> _OnInitData;
    public event Action<GameObject, GameObject, int, bool> _OnUpdateCharacter;
    public event Action _OnInitialize;
    public event Action<int, bool> _OnAddDamage;
    public event Action<bool> _OnAddStatus;
    public event Action<bool> _OnReset;

    [SerializeField]
    PhotonPropertyGameObject propertyPhoton;
    [SerializeField]
    AudioListener audioListener;

    bool isInit = false;
    bool isPropertyInit = false;
    GameObject weapon;
    GameObject hair;
    int level = 1;

    object[] characterData;
    private void Awake()
    {
        propertyPhoton._OnCompleteInit += OnPropertyInitComplete;
        if (photonView.IsMine)
            NetworkManager.Inst._OnPlayerStatusRoomChange += OnPlayerEnterRoom;
        else
        {
            if(audioListener)
                audioListener.enabled = false;
        }
    }
    [PunRPC]
    protected void RPC_Character_OnInit(object[] data)
    {
        int[] lastData = new int[data.Length - 2];
        for(int i = 0; i < data.Length - 2; i++)
        {
            lastData[i] = (int)data[i + 2];
        }
        weapon = PhotonView.Find((int)data[0]).gameObject;
        hair = PhotonView.Find((int)data[1]).gameObject;
        level = (int)data[7];

        _OnInitData?.Invoke(lastData);
        isInit = true;
        if (isInit && isPropertyInit && !photonView.IsMine)
            _OnUpdateCharacter?.Invoke(weapon,hair,level, false);
    }
    [PunRPC]
    protected void RPC_Character_Event(int eventcode, object[] data)
    {
        switch ((EVENT)eventcode)
        {
            case EVENT.TAKE_DAMAGE:
                _OnAddDamage?.Invoke((int)data[0], true);
                break;
            case EVENT.LEVEL_UP:
                _OnAddStatus?.Invoke(true);
                break;
            case EVENT.REVIVE:
                _OnReset?.Invoke(true);
                break;
        }
        Debug.Log($"<color=yellow>EVENT</color> - Player: {(EVENT)eventcode}");
    }

    public void UpdateNetworkEvent(EVENT eventcode,object[] data = null)
    {
        photonView.RPC(nameof(RPC_Character_Event), RpcTarget.Others, (int)eventcode, data as object);       
    }
    public void SetNetworkData(GameObject weapon,GameObject hair,ref int[] data)
    {
        if (!photonView.IsMine) return;

        characterData = new object[data.Length];
        data[0] = weapon.GetComponent<PhotonView>().ViewID;
        data[1] = hair.GetComponent<PhotonView>().ViewID;
        for(int i = 0; i < data.Length; i++)
        {
            characterData[i] = data[i];
        }
        photonView.RPC(nameof(RPC_Character_OnInit), RpcTarget.Others, characterData as object);
    }
    private void OnPlayerEnterRoom(Player player, bool value)
    {
        if (value)
        {
            photonView.RPC(nameof(RPC_Character_OnInit), player, characterData as object);
        }
    }
    private void OnPropertyInitComplete()
    {
        _OnInitialize?.Invoke();
        isPropertyInit = true;
        if (isInit && isPropertyInit && !photonView.IsMine)
            _OnUpdateCharacter?.Invoke(weapon, hair, level, false);
    }

    private void OnDestroy()
    {
        if (photonView.IsMine)
            NetworkManager.Inst._OnPlayerStatusRoomChange -= OnPlayerEnterRoom;
    }
}

using MoveStopMove.Core;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonBaseBullet : MonoBehaviourPun
{
    // Start is called before the first frame update
    public event Action<Vector3, float, BaseCharacter, bool, float, bool> _OnFire;
    [PunRPC]
    protected void RPC_OnFire(Vector3 direction, float range , int characterId, bool isSpecial ,float speedRatio)
    {
        BaseCharacter character = PhotonView.Find(characterId).GetComponent<BaseCharacter>(); //DEV: Cache
        _OnFire?.Invoke(direction, range, character, isSpecial, speedRatio, true);
        //Debug.Log("Sync On Fire");
    }
    public void SetNetworkData(Vector3 direction, float range, GameObject parentCharacter, bool isSpecial, float speedRatio)
    {
        int characterId = parentCharacter.GetComponent<PhotonView>().ViewID;
        photonView.RPC(nameof(RPC_OnFire), RpcTarget.Others, direction, range, characterId, isSpecial, speedRatio);
        //Debug.Log("Call RPC Fire");
    }
}

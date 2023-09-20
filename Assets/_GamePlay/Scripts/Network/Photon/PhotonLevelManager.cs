using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonLevelManager : MonoBehaviourPun
{
    private void Awake()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            NetworkManager.Inst.Destroy(gameObject);
        }
    }
}

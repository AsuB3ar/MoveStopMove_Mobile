using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoveStopMove.Manager;
using Photon.Pun;

public class CanvasPvpLobby : UICanvas
{
    [SerializeField]
    protected InputField createInput;
    [SerializeField]
    protected InputField joinInput;
    [SerializeField]
    protected InputField nameInput;
    [SerializeField]
    protected Button createButton;
    [SerializeField]
    protected Button joinButton;
    [SerializeField]
    protected Button leaveButton;
    [SerializeField]
    protected Text statusTxt;

    private void Awake()
    {
        statusTxt.text = "Status: Lobby";
        createButton.onClick.AddListener(OnClickCreateRoom);
        joinButton.onClick.AddListener(OnClickJoinRoom);
        leaveButton.onClick.AddListener(OnClickLeavePvp);
        NetworkManager.Inst.ClearEvent();

        NetworkManager.Inst._OnJoinedRoom += () =>
        {
            GameplayManager.Inst.GameMode = GAMECONST.GAMEPLAY_MODE.STANDARD_PVP;           
            PrefabManager.Inst.ChangeMode(GAMECONST.GAMEPLAY_MODE.STANDARD_PVP);
            SceneManager.Inst.LoadPhotonScene(GAMECONST.INIT_PVP_RESOUCRCES_SCENE);
            statusTxt.text = "Status: Room";
        };
    }

    private void OnClickJoinRoom()
    {
        NetworkManager.Inst.JoinRoom(joinInput.text, nameInput.text);
    }

    protected void OnClickCreateRoom()
    {
        NetworkManager.Inst.CreateRoom(createInput.text, nameInput.text);
    }

    protected void OnClickLeavePvp()
    {
        NetworkManager.Inst.DisconnectMaster();
        NetworkManager.Inst._OnDisconnectServer += () =>
        {
            SceneManager.Inst.LoadScene(GAMECONST.STANDARD_PVE_SCENE);
            NetworkManager.Inst.ClearEvent();
        };
    }
}

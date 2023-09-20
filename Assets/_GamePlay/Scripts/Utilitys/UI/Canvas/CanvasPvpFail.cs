using MoveStopMove.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilitys.Timer;

public class CanvasPvpFail : UICanvas
{
    private const int REVIVE_TIME = 5;
    [SerializeField]
    TMP_Text timeText;
    [SerializeField]
    Button leaveButton;

    STimer timer;
    float reviveTime;
    private void Awake()
    {
        timer = TimerManager.Inst.PopSTimer();
        NetworkManager.Inst._OnLeftRoom += LeftRoom;
        leaveButton.onClick.AddListener(() =>
        {
            PrefabManager.Inst.ChangeMode(GAMECONST.GAMEPLAY_MODE.STANDARD_PVE);
            NetworkManager.Inst.LeaveRoom();
        });
    }
    public override void Open()
    {
        base.Open();
        reviveTime = REVIVE_TIME;
        timeText.text = reviveTime.ToString();
        timer.Start(1f, () => 
        {
            reviveTime -= 1;
            timeText.text = reviveTime.ToString();
            if(reviveTime <= 0)
            {
                timer.Stop();
                LevelManager.Inst.RevivePlayer();
                UIManager.Inst.OpenUI(UIID.UICGamePlay);
                Close();
            }
        }, true);
    }

    public override void Close()
    {
        base.Close();
        timer.Stop();
    }
    private void LeftRoom()
    {
        UIManager.Inst.GetUI<CanvasGameplay>(UIID.UICGamePlay).Close();
        UIManager.Inst.GetUI<CanvasGameplay>(UIID.UICGamePlay).ClearTargets();
        NetworkManager.Inst._OnLeftRoom -= LeftRoom;
        SceneManager.Inst.LoadPhotonScene(GAMECONST.PVP_LOBBY_SCENE);
        Close();
    }
    private void OnDestroy()
    {
        TimerManager.Inst.PushSTimer(timer);
    }
}

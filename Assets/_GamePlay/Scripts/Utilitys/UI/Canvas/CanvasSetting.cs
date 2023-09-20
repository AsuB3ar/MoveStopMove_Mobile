using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoveStopMove.Manager;

public class CanvasSetting : UICanvas
{
    private readonly List<float> RESOLUTIONS = new List<float>() { 1f,9f/16, 10f/16, 3f/4  };

    public void HomeButton()
    {
        switch (GameplayManager.Inst.GameMode)
        {
            case GAMECONST.GAMEPLAY_MODE.STANDARD_PVE:
                Close();
                UIManager.Inst.OpenUI(UIID.UICMainMenu);
                if (UIManager.Inst.IsOpenedUI(UIID.UICGamePlay))
                    UIManager.Inst.GetUI(UIID.UICGamePlay).Close();
                break;
            case GAMECONST.GAMEPLAY_MODE.STANDARD_PVP:
                PrefabManager.Inst.ChangeMode(GAMECONST.GAMEPLAY_MODE.STANDARD_PVE);
                NetworkManager.Inst.LeaveRoom();
                NetworkManager.Inst._OnLeftRoom += LeftRoom;
                break;
        }
        
    }
    private void LeftRoom()
    {
        UIManager.Inst.GetUI(UIID.UICPvpMainMenu).Close();
        UIManager.Inst.GetUI<CanvasGameplay>(UIID.UICGamePlay).ClearTargets();
        UIManager.Inst.GetUI(UIID.UICGamePlay).Close();
        NetworkManager.Inst._OnLeftRoom -= LeftRoom;
        SceneManager.Inst.LoadPhotonScene(GAMECONST.PVP_LOBBY_SCENE);
        Close();
    }
    public void SetResolution(int value)
    {
        GameManager.Inst.SetResolution(RESOLUTIONS[value]);             
    }

    public void SetVolume(float value)
    {
        SoundManager.Inst.SetVolume(value);
    }
}

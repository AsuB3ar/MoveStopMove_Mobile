using MoveStopMove.Manager;
using UnityEngine;

public class CanvasPvpMainMenu : UICanvas
{
    public void PlayGameButton()
    {
        UIManager.Inst.OpenUI(UIID.UICGamePlay);
        SoundManager.Inst.PlaySound(SoundManager.Sound.Button_Click);
        GameManager.Inst.StartGame();
        Close();
    }


    public void SettingButton()
    {
        UIManager.Inst.OpenUI(UIID.UICSetting);
        SoundManager.Inst.PlaySound(SoundManager.Sound.Button_Click);
    }

    public override void Open()
    {
        base.Open();
            //GameplayManager.Inst.PlayerScript.Reset();
        GameManager.Inst.StopGame();
        LevelManager.Inst.OpenLevel(GameManager.Inst.GameData.CurrentRegion);
        SoundManager.Inst.PlaySound(SoundManager.Sound.Button_Click);
        GameplayManager.Inst.SetCameraPosition(CameraPosition.MainMenu);
        UpdateData();
    }

    public void UpdateData()
    {

    }
}

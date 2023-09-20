using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoveStopMove.Manager;
using MoveStopMove.Core.Data;
using Photon.Pun;

public class CanvasMainMenu : UICanvas
{
    [SerializeField]
    TMP_Text descriptionPlayText;
    [SerializeField]
    TMP_Text cash;


    GameData Data;
    private const string ZONE = "Zone:";
    private const string BEST = " - Best:#";

    public void Awake()
    {
        Data = GameManager.Inst.GameData;
    }
    public void PlayGameButton()
    {
        UIManager.Inst.OpenUI(UIID.UICGamePlay);
        SoundManager.Inst.PlaySound(SoundManager.Sound.Button_Click);
        GameManager.Inst.StartGame();
        Close();
    }

    public void ShopSkinButton()
    {
        UIManager.Inst.OpenUI(UIID.UICShopSkin);
        SoundManager.Inst.PlaySound(SoundManager.Sound.Button_Click);
        Close();
    }

    public void ShopWeaponButton()
    {
        CanvasShopWeapon shopCanvas = UIManager.Inst.OpenUI<CanvasShopWeapon>(UIID.UICShopWeapon, RenderMode.ScreenSpaceCamera);
        shopCanvas.SetCanvasCamera(GameplayManager.Inst.PlayerCamera);
        SoundManager.Inst.PlaySound(SoundManager.Sound.Button_Click);
        Close();
    }

    public void PlayPvpButton()
    {
        NetworkManager.Inst.ClearEvent(); //NOTE: Clear All Events Before
        LevelManager.Inst.DestructLevel();
        PhotonNetwork.ConnectUsingSettings();
        SoundManager.Inst.PlaySound(SoundManager.Sound.Button_Click);       
        NetworkManager.Inst._OnConnectedToMaster += () => PhotonNetwork.JoinLobby();
        NetworkManager.Inst._OnJoinedLobby += () => SceneManager.Inst.LoadScene(GAMECONST.PVP_LOBBY_SCENE);       
        NetworkManager.Inst.ConnectToServer();
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
        LevelManager.Inst.OpenLevel(Data.CurrentRegion);
        SoundManager.Inst.PlaySound(SoundManager.Sound.Button_Click);            
        GameplayManager.Inst.SetCameraPosition(CameraPosition.MainMenu);
        UpdateData();
    }

    public void UpdateData()
    {
        string des = ZONE + (Data.CurrentRegion + 1).ToString() + BEST + Data.HighestRank.ToString();
        descriptionPlayText.text = des;
        cash.text = Data.Cash.ToString();
    }

}

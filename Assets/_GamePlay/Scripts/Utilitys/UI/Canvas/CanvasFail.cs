using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MoveStopMove.Manager;
using MoveStopMove.Core.Data;
using MoveStopMove.Core;

public class CanvasFail : UICanvas
{
    [SerializeField]
    TMP_Text text;
    [SerializeField]
    TMP_Text cashText;
    GameData Data;

    private void Awake()
    {
        Data = GameManager.Inst.GameData;
    }
    public void CloseButton()
    {
        UIManager.Inst.OpenUI(UIID.UICMainMenu);
        SoundManager.Inst.PlaySound(SoundManager.Sound.Button_Click);
        Close();
    }

    public void SetRank(int rank)
    {
        text.text = '#'+rank.ToString();
        if(rank < Data.HighestRank)
        {
            Data.SetIntData(Player.P_HIGHTEST_SCORE, ref Data.HighestRank, rank);
        }
    }
    public void SetCash(int cash)
    {
        cashText.text = '+' + cash.ToString();
    }
}

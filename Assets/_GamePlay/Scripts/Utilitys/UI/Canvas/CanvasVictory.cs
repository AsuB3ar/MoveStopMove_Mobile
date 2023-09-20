using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using MoveStopMove.Manager;
using MoveStopMove.Core.Data;
using MoveStopMove.Core;

public class CanvasVictory : UICanvas
{
    public TMP_Text cashText;
    public int currentLevel = 1;
    GameData Data;

    private void Awake()
    {
        Data = GameManager.Inst.GameData;
    }
    public void SetCash(int score)
    {
        cashText.text = '+' + score.ToString();
    }
    public void SetCurrentLevel(int currentLevel)
    {
        this.currentLevel = currentLevel;
    }

    public void CloseButton()
    {
        UIManager.Inst.OpenUI(UIID.UICMainMenu);
        SoundManager.Inst.PlaySound(SoundManager.Sound.Button_Click);
        Close();
    }

    public void NextLevelButton()
    {
        currentLevel++;
        UIManager.Inst.OpenUI(UIID.UICGamePlay);
        LevelManager.Inst.OpenLevel(currentLevel);
        SoundManager.Inst.PlaySound(SoundManager.Sound.Button_Click);

        Data.SetIntData(Player.P_CURRENT_REGION,ref Data.CurrentRegion, currentLevel);
        Data.SetIntData(Player.P_HIGHTEST_SCORE, ref Data.HighestRank, 100);
        GameManager.Inst.StartGame();
        
        Close();
    }

    public void PlayAgainButton()
    {
        UIManager.Inst.OpenUI(UIID.UICGamePlay);
        LevelManager.Inst.OpenLevel(currentLevel);
        SoundManager.Inst.PlaySound(SoundManager.Sound.Button_Click);

        Data.SetIntData(Player.P_CURRENT_REGION, ref Data.CurrentRegion, currentLevel);
        Data.SetIntData(Player.P_HIGHTEST_SCORE, ref Data.HighestRank, 1);
        GameManager.Inst.StartGame();
        Close();
    }
}

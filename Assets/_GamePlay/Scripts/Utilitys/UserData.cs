using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Numerics;
using System.Globalization;

[CreateAssetMenu(fileName = "UserData", menuName = "ScriptableObjects/UserData", order = 1)]
public class UserData : ScriptableObject
{
#if UNITY_EDITOR
    [Header(" ----Test Data----")]

    public bool IsTestCheckData = false;
#endif

    [Header("----Data----")]

    public int PlayingLevel = 0;

    public string Cash;
    public bool removeAds = false;

    public bool musicIsOn = true;
    public bool vibrationIsOn = true;
    public bool fxIsOn = true;
    public bool tutorialed = false;

    public int maxLevelMeleeUnlock = 0;
    public int maxLevelRangeUnlock = 0;

    public int meleeHaveOwned = 0;
    public int rangeHaveOwned = 0;

    public string lastTimePlay;


    /// <summary>
    ///  0 = lock , 1 = unlock , 2 = selected
    /// </summary>
    /// <param name="data"></param>
    /// <param name="ID"></param>
    /// <param name="state"></param>
    

    public void OnInitData() 
    {
#if UNITY_EDITOR
        if (IsTestCheckData)
        {
            return;
        }
#endif

        PlayingLevel = PlayerPrefs.GetInt(Key_PlayingLevel, 1);
        Cash = PlayerPrefs.GetString(Key_Cash, "50");
        musicIsOn = PlayerPrefs.GetInt(Key_MusicIsOn, 1) == 1;
        vibrationIsOn = PlayerPrefs.GetInt(Key_VibrationIsOn, 1) == 1;
        fxIsOn = PlayerPrefs.GetInt(Key_FxIsOn, 1) == 1;
        removeAds = PlayerPrefs.GetInt(Key_RemoveAds, 0) == 1;
        tutorialed =  PlayerPrefs.GetInt(Key_Tutorial, 0) == 1;
        lastTimePlay = PlayerPrefs.GetString(Key_Last_Time_Play, System.DateTime.Now.ToString(CultureInfo.InvariantCulture));

        maxLevelMeleeUnlock = PlayerPrefs.GetInt(Key_Max_Level_Melee_Unlock, 1);
        maxLevelRangeUnlock = PlayerPrefs.GetInt(Key_Max_Level_Range_Unlock, 1);

        meleeHaveOwned = PlayerPrefs.GetInt(Key_Melee_Have_Owned, 0);
        rangeHaveOwned = PlayerPrefs.GetInt(Key_Range_Have_Owned, 0);

    }

    public void OnResetData()
    {
        PlayerPrefs.DeleteAll();
        OnInitData();
    }

    public const string Key_PlayingLevel = "Level";
    public const string Key_Cash = "Cash";
    public const string Key_FxIsOn = "SoundIsOn";
    public const string Key_MusicIsOn = "MusicIsOn";
    public const string Key_VibrationIsOn = "VibrationIsOn";
    public const string Key_RemoveAds = "RemoveAds";
    public const string Key_Tutorial = "Tutorial";
    public const string Key_Last_Time_Play = "Key_Last_Time_Play";

    public const string Key_Slot_Type_ = "KeySlotType_";
    public const string Key_Slot_Level_ = "KeySlotLevel_";

    public const string Key_Max_Level_Melee_Unlock = "Key_Max_Level_Melee_Unlock";
    public const string Key_Max_Level_Range_Unlock = "Key_Max_Level_Range_Unlock";

    public const string Key_Melee_Have_Owned = "Key_Melee_Have_Owned";
    public const string Key_Range_Have_Owned = "Key_Range_Have_Owned";
}



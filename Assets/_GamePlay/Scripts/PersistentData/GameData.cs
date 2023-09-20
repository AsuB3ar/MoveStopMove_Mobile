using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoveStopMove.Manager;

namespace MoveStopMove.Core.Data
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Data/Game", order = 1)]
    public class GameData : ScriptableObject
    {
        public const string POOL_ID_ITEM_NAME = "PoolID";
        public const string PANT_SKIN_ITEM_NAME = "PantSkin";


        #region Player Data
        #region Stats
        public float Speed = 3;
        public int Weapon;
        #endregion

        #region Skin
        public int Color;
        public int Pant;
        public int Hair;
        public int Set = 0;
        #endregion

        #region Poverty
        public int HighestRank = 100;
        public int CurrentRegion = 0;
        public int Cash = 0;

        public Dictionary<PoolID, int> PoolID2State = new Dictionary<PoolID, int>();
        public Dictionary<PantSkin, int> PantSkin2State = new Dictionary<PantSkin, int>();
        private bool isDirty = false;
        #endregion
        #endregion

        private void OnEnable()
        {
            isDirty = false;
        }
        public void SetDataState(string data, int ID, int state)
        {
            PlayerPrefs.SetInt(data + ID, state);
        }

        /// <summary>
        ///  0 = lock , 1 = unlock , 2 = selected
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ID"></param>
        /// <param name="state"></param>
        public int GetDataState(string data, int ID, int defaultID = 0)
        {
            return PlayerPrefs.GetInt(data + ID, defaultID);
        }

        /// <summary>
        ///  0 = lock , 1 = unlock , 2 = selected
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ID"></param>
        /// <param name="state"></param>
        public void SetDataState(string data, int ID, ref List<int> variable, int state)
        {
            variable[ID] = state;
            PlayerPrefs.SetInt(data + ID, state);
        }

        /// <summary>
        /// Key_Name
        /// if(bool) true == 1
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        public void SetIntData(string data, ref int variable, int value)
        {
            variable = value;
            PlayerPrefs.SetInt(data, value);
        }

        public void SetBoolData(string data, ref bool variable, bool value)
        {
            variable = value;
            PlayerPrefs.SetInt(data, value ? 1 : 0);
        }

        public void SetFloatData(string data, ref float variable, float value)
        {
            variable = value;
            PlayerPrefs.GetFloat(data, value);
        }

        public void SetStringData(string data, ref string variable, string value)
        {
            variable = value;
            PlayerPrefs.SetString(data, value);
        }
        public void OnInitData()
        {
            if (isDirty) return;
            isDirty = true;
            #region Player Data
            #region Stats
            Speed = PlayerPrefs.GetFloat(Player.P_SPEED,3);
            Weapon = PlayerPrefs.GetInt(Player.P_WEAPON, (int)PoolID.Weapon_Arrow);
            #endregion

            #region Skins
            Color = PlayerPrefs.GetInt(Player.P_COLOR,0);
            Pant = PlayerPrefs.GetInt(Player.P_PANT,(int)PantSkin.Batman);
            Hair = PlayerPrefs.GetInt(Player.P_HAIR,(int)PoolID.Hair_Arrow);
            Set = PlayerPrefs.GetInt(Player.P_SET,0);
            #endregion
            #region Poverty
            HighestRank = PlayerPrefs.GetInt(Player.P_HIGHTEST_SCORE, 50);
            CurrentRegion = PlayerPrefs.GetInt(Player.P_CURRENT_REGION, 0);
            Cash = PlayerPrefs.GetInt(Player.P_CASH, 0);

            List<PoolID> poolIdItems = GameplayManager.Inst.HairSkins;
            List<PoolID> weaponItems = GameplayManager.Inst.WeaponNames;
            List<PantSkin> pantSkinItems = GameplayManager.Inst.PantSkins;
            

            for (int i = 0; i < poolIdItems.Count; i++)
            {
                PoolID2State.Add(poolIdItems[i], GetDataState(POOL_ID_ITEM_NAME, (int)poolIdItems[i], 0));
            }

            for(int i = 0; i < weaponItems.Count; i++)
            {
                PoolID2State.Add(weaponItems[i], GetDataState(POOL_ID_ITEM_NAME, (int)weaponItems[i], 0));
            }

            for(int i = 0; i < pantSkinItems.Count; i++)
            {
                PantSkin2State.Add(pantSkinItems[i], GetDataState(PANT_SKIN_ITEM_NAME, (int)pantSkinItems[i], 0));
            }

            PoolID2State[PoolID.Hair_Arrow] = 1;
            PoolID2State[PoolID.Weapon_Arrow] = 1;
            PantSkin2State[PantSkin.Batman] = 1;

            #endregion
            #endregion
        }

        public void ResetData()
        {
            PlayerPrefs.DeleteAll();
            PoolID2State.Clear();
            PantSkin2State.Clear();
            OnInitData();
        }
    }
}
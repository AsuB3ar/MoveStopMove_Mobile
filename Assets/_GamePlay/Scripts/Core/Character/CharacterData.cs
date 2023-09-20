
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.Core.Data {
    public class CharacterData : ScriptableObject
    {
        public const float BASE_ATTACK_RANGE = 3f;
        public const float BASE_SPEED = 3;

        #region Stats
        private float size = 1;
        private int level = 1;

        public float Speed => BASE_SPEED * (size * 0.2f + 0.8f);
        public int Score = 0;

        public float Size => size;
        public float BaseAttackRange = BASE_ATTACK_RANGE;
        public int Hp = 1;
        public int AttackCount = 1;
        public int Level
        {
            get => level;
            set
            {
                level = value;
                if (value < 10)
                {
                    size = Mathf.Pow(1.1f, value - 1);
                }
                else if(value < 20)
                {
                    size = Mathf.Pow(1.1f, 9) * Mathf.Pow(1.05f, value - 10);
                }
                else
                {
                    size = Mathf.Pow(1.1f, 9) * Mathf.Pow(1.05f, 10) * Mathf.Pow(1.02f, value - 20);
                }

            }
        }
        public float AttackRange => BaseAttackRange * Size;

        public int Weapon;
        #endregion

        #region Skin
        public int Color;
        public int Pant;
        public int Hair;
        public int Set = 0;
        #endregion

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.Core.Character.LogicSystem
{
    public class LogicParameter : AbstractParameterSystem
    {
        public Vector3 Velocity;
        public Vector3 MoveDirection;

        public bool IsGrounded;
        public bool IsHaveGround;
        public Transform PlayerTF;
        public BaseCharacter TargetCharacter;
        #region Attack State Variable

        private bool isSpecialAttack = false;
        public bool Gift
        {
            get
            {
                if (isSpecialAttack)
                {
                    isSpecialAttack = false;
                    return true;
                }
                else
                {
                    return false;
                }

            }
            set
            {
                if (value)
                {
                    isSpecialAttack = value;
                }
            }
        }
        #endregion
    }
}
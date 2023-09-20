using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MoveStopMove.Core.Character.LogicSystem
{
    using System;
    public class LogicEvent : ScriptableObject
    {
        public Action<Vector3> SetVelocity;
        public Action<GAMECONST.PHYSIC_OTYPE,Quaternion> SetRotation;
        public Action<GAMECONST.PHYSIC_OTYPE, Vector3> SetSmoothRotation;

        public Action<string, bool> SetBool_Anim;
        public Action<string, float> SetFloat_Anim;
        public Action<string, int> SetInt_Anim;
        public Action<Vector3, float> DealDamage;
        public Action<Vector3, float, bool> SpecialDealDamage;
        public Action<bool> SetPhysicModuleActive;
        public Action<BaseCharacter, bool> SetTargetIndicatorPosition;
        public Action OnDespawn;

        public Action<Collider> CollideGift;
        public Action EndGiftBonus;
        
    }
}
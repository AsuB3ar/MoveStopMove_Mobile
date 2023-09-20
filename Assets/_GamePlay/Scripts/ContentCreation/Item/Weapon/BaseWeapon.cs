using MoveStopMove.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WeaponType
{
    Normal  = 0,
    Has3Ray = 1
}
namespace MoveStopMove.ContentCreation.Weapon
{
    using Manager;
    public abstract class BaseWeapon : Item
    {
        float Range;
        [SerializeField]
        protected PoolID BulletPoolName;
        [SerializeField]
        protected WeaponType WeaponType;
        [SerializeField]
        protected Transform firePoint;
        
        [HideInInspector]
        public BaseCharacter Character;
        public PoolID Name => data.poolID;
             
        public virtual void DealDamage(Vector3 direction, float range, float scale, bool isSpecial, float speedRatio)
        {
            SoundManager.Inst.PlaySound(SoundManager.Sound.Weapon_Throw,gameObject.transform.position);
        }
    }
}
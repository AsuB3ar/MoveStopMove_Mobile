using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.ContentCreation.Weapon
{
    using MoveStopMove.Manager;
    using Utilitys;
    public class ArrowWeapon : BaseWeapon
    {
        // Start is called before the first frame update
        private void Start()
        {
            SetTranformData();
        }
        public override void DealDamage(Vector3 direction, float range, float size, bool isSpecial,float speedRatio)
        {
            base.DealDamage(direction, range, size,isSpecial,speedRatio);
            if(WeaponType == WeaponType.Has3Ray)
            {

                float angle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up) + 90;


                Vector3 direction1 = MathHelper.AngleToVector(angle + 30);
                direction1.z = direction1.y;
                direction.y = 0;
                Vector3 direction2 = MathHelper.AngleToVector(angle - 30);
                direction2.z = direction2.y;
                direction2.y = 0;



                GameObject bullet = PrefabManager.Inst.PopFromPool(BulletPoolName);
                bullet.transform.position = firePoint.position;
                bullet.transform.localScale = Vector3.one * size;

                GameObject bullet1 = PrefabManager.Inst.PopFromPool(BulletPoolName);
                bullet1.transform.position = firePoint.position;
                bullet1.transform.localScale = Vector3.one * size;

                GameObject bullet2 = PrefabManager.Inst.PopFromPool(BulletPoolName);
                bullet2.transform.position = firePoint.position;
                bullet2.transform.localScale = Vector3.one * size;

                BaseBullet bulletScript = Cache.GetBaseBullet(bullet);
                bulletScript.OnFire(direction, range, Character, isSpecial, speedRatio);

                BaseBullet bulletScript1 = Cache.GetBaseBullet(bullet1);
                bulletScript1.OnFire(direction1, range, Character, isSpecial, speedRatio);

                BaseBullet bulletScript2 = Cache.GetBaseBullet(bullet2);
                bulletScript2.OnFire(direction2, range, Character, isSpecial, speedRatio);
            }        
        }
    }
}
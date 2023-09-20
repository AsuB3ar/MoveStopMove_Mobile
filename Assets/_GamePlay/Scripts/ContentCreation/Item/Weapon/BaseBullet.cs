using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.ContentCreation.Weapon
{
    using Core;
    using Manager;
    public enum BulletType
    {
        Normal = 0,
        HorizontalRotation = 1
    }
    public class BaseBullet : MonoBehaviour
    {
        private const float AMPLIFY_PARAMETER = 60;
        private const float SPECIAL_MAX_SCALE = 3f;

        float range;
        BaseCharacter parentCharacter;
        [SerializeField]
        LayerMask characterLayer;
        [SerializeField]
        LayerMask obstanceLayer;
        [SerializeField]
        BulletType Type;
        [SerializeField]
        PoolID poolName;

        [SerializeField]
        float rotationSpeed = 30f;
        [SerializeField]
        float speed = 0.1f;
        [SerializeField]
        PhotonBaseBullet photon;
        float speedRatio = 1f;
        

        Vector3 direction = Vector3.zero;
        bool isSpecial;
        bool isStop = false;

        Vector3 specialScale;       
        float currentSpeed;
        float specialSpeed;
        float lastSpeed => currentSpeed * Time.fixedDeltaTime * AMPLIFY_PARAMETER * speedRatio;
        [HideInInspector]
        public Collider SelfCharacterCollider;
        private void OnEnable()
        {
            isStop = false;
        }
        private void Awake()
        {
            currentSpeed = speed;
            if (photon != null)
                photon._OnFire += OnFire;
        }
        private void FixedUpdate()
        {
            //if (photon && !photon.photonView.IsMine) return;
            if (range < 0)
            {
                CheckPushToPool();
            }
            else
            {
                range -= lastSpeed;
            }

            if (isStop)
                return;
            //Code When Special
            if (isSpecial)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, specialScale, 0.08f);
                currentSpeed = Mathf.Lerp(currentSpeed, specialSpeed, 0.1f);
            }

            if(Type == BulletType.HorizontalRotation)
            {
                transform.Rotate(0, 0, -rotationSpeed * Time.fixedDeltaTime * AMPLIFY_PARAMETER,Space.Self);
            }
            if(direction.sqrMagnitude > 0.001)
            {
                transform.Translate(direction * lastSpeed,Space.World);
            }                   
        }

        private void CheckPushToPool()
        {
            if (!photon)
                PrefabManager.Inst.PushToPool(this.gameObject, poolName, false);
            else
            {
                if (photon.photonView.IsMine)
                    PrefabManager.Inst.PushToPool(this.gameObject, poolName, false);
            }
        }

        public void OnHit(BaseCharacter character)
        {
            if (photon && !photon.photonView.IsMine) return;
            if (parentCharacter == null) return;
            if(character != parentCharacter)
            {
                if (!character.IsDie)
                {
                    CheckPushToPool();
                    character.TakeDamage(1);
                    parentCharacter.AddStatus(); 
                }               
            }
        }

        public void OnFire(Vector3 direction, float range, BaseCharacter parentCharacter, bool isSpecial = false, float speedRatio = 1, bool isRpcCall = false)
        {
            direction.y = 0;
            this.direction = direction.normalized;          
            this.range = range - lastSpeed * 6;
            this.parentCharacter = parentCharacter;
            this.isSpecial = isSpecial;
            this.speedRatio = speedRatio;

            if (isSpecial)
            {
                specialScale = transform.localScale * SPECIAL_MAX_SCALE;
                specialSpeed = speed * BaseCharacter.GIFT_BONUS * speedRatio;
            }
            else
            {
                currentSpeed = speed;
            }

            if(Type == BulletType.Normal)
            {
                direction.y = 1;
                transform.localRotation = Quaternion.LookRotation(Vector3.up,-direction);
            }
            if (photon && !isRpcCall)
            {
                photon.SetNetworkData(direction, range, parentCharacter.gameObject, isSpecial, speedRatio);
            }
        }

        private void OnTriggerEnter(Collider col)
        {
            if(Mathf.Pow(2, col.gameObject.layer) == characterLayer)
            {
                OnHit(Cache.GetBaseCharacter(col));
            }
            else if(Mathf.Pow(2, col.gameObject.layer) == obstanceLayer)
            {
                isStop = true;
            }
        }
    }
}
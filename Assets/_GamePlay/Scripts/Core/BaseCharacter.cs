using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.Core
{
    using Manager;
    using Utilitys;
    using Utilitys.Timer;
    using MoveStopMove.Core.Data;
    using MoveStopMove.Core.Character.WorldInterfaceSystem;
    using MoveStopMove.Core.Character.NavigationSystem;
    using MoveStopMove.Core.Character.PhysicSystem;
    using MoveStopMove.Core.Character.LogicSystem;
    using MoveStopMove.ContentCreation;
    using ContentCreation.Weapon;
    using System;
    public enum CharacterType
    {
        Player = 0,
        Enemy = 1
    }

    public class BaseCharacter : MonoBehaviour,IInit,IDespawn
    {
        public event Action<BaseCharacter> OnDie;
        public const float GIFT_BONUS = 1.6f;

        protected VisualEffectController VFX_Hit;
        protected VisualEffectController VFX_AddStatus;
        protected CharacterData data = null;
        protected CharacterData Data
        {
            get
            {
                if(data == null)
                {
                    data = ScriptableObject.CreateInstance(typeof(CharacterData)) as CharacterData;
                }
                return data;
            }
        }

        [SerializeField]
        protected SkinnedMeshRenderer meshCharacter;
        [SerializeField]
        protected SkinnedMeshRenderer meshPant;
        [SerializeField]
        protected GameObject hair;
        [SerializeField]
        protected Transform SensorTF;
        [SerializeField]
        protected Transform ContainWeaponTF;
        [SerializeField]
        protected Transform ContainHairTF;
        

        [SerializeField]
        protected WorldInterfaceModule WorldInterfaceModule;
        [SerializeField]
        protected AbstractNavigationModule NavigationModule;
        [SerializeField]
        protected AbstractLogicModule LogicModule;
        [SerializeField]
        protected AbstractPhysicModule PhysicModule;
        [SerializeField]
        protected AnimationModule AnimModule;


        protected CharacterWorldInterfaceSystem WorldInterfaceSystem;
        protected CharacterNavigationSystem NavigationSystem;
        protected CharacterLogicSystem LogicSystem;
        protected CharacterPhysicSystem PhysicSystem;     

        public BaseWeapon Weapon;

        protected STimer dieTimer;
        private float despawnTime = 2f;
        private List<float> dieTimes;
        private List<Action> dieActions;
        public bool IsDie
        {
            get
            {
                if (Data.Hp > 0) return false;
                else return true;
            }
        }
        public float Size => Data.Size;
        public float AttackRange => Data.AttackRange;
        public int Level => Data.Level;
        public GameColor Color => (GameColor)Data.Color;
        public bool IsRun => ((CharacterAI)NavigationModule).StateMachine.IsStarted;


        protected virtual void Awake()
        {
            WorldInterfaceSystem = new CharacterWorldInterfaceSystem(WorldInterfaceModule);
            NavigationSystem = new CharacterNavigationSystem(NavigationModule);
            LogicSystem = new CharacterLogicSystem(LogicModule);
            PhysicSystem = new CharacterPhysicSystem(PhysicModule);

            NavigationSystem.SetCharacterInformation(transform, SensorTF, GetInstanceID());
            dieTimer = TimerManager.Inst.PopSTimer();
            CalculateActionAndTime();
            //NOTE: When change wepon need to set this line of code
            
        }


        public virtual void OnInit()
        {
            PhysicModule.SetActive(true);
            transform.localScale = Vector3.one * Data.Size;
            PhysicModule.SetRotation(GAMECONST.PHYSIC_OTYPE.Model, Quaternion.Euler(0, 0, 0));
            PhysicModule.SetRotation(GAMECONST.PHYSIC_OTYPE.Sensor, Quaternion.Euler(0, 0, 0));          
            Data.AttackCount = 0;
            OnEnable();
            ((CharacterLogicModule)LogicModule).StartStateMachine();            
        }
        

        public virtual void OnDespawn()
        {
        }

        public virtual void Run()
        {        
        }

        public virtual void Stop()
        {
        }
        protected virtual void OnEnable()
        {
            if (LogicSystem.Event.SetVelocity != null) return;
            #region Update Data Event
            WorldInterfaceSystem.OnUpdateData += NavigationSystem.ReceiveInformation;
            WorldInterfaceSystem.OnUpdateData += LogicSystem.ReceiveInformation;

            NavigationSystem.OnUpdateData += LogicSystem.ReceiveInformation;
            PhysicSystem.OnUpdateData += LogicSystem.ReceiveInformation;

            LogicSystem.Event.SetVelocity += PhysicSystem.SetVelocity;
            LogicSystem.Event.SetRotation += PhysicSystem.SetRotation;
            LogicSystem.Event.SetPhysicModuleActive += PhysicModule.SetActive;

            LogicSystem.Event.SetSmoothRotation += PhysicSystem.SetSmoothRotation;
            LogicSystem.Event.SetBool_Anim += AnimModule.SetBool;
            LogicSystem.Event.SetFloat_Anim += AnimModule.SetFloat;
            LogicSystem.Event.SetInt_Anim += AnimModule.SetInt;
            LogicSystem.Event.DealDamage += DealDamage;
            LogicSystem.Event.SpecialDealDamage += DealDamage;
            LogicSystem.Event.CollideGift += OnCollideGift;
            LogicSystem.Event.EndGiftBonus += OnEndGiftBonus;

            AnimModule.UpdateEventAnimationState += LogicSystem.ReceiveInformation;
            #endregion           
        }

        protected virtual void OnDisable()
        {
            #region Update Data Event
            WorldInterfaceSystem.OnUpdateData -= NavigationSystem.ReceiveInformation;
            WorldInterfaceSystem.OnUpdateData -= LogicSystem.ReceiveInformation;

            NavigationSystem.OnUpdateData -= LogicSystem.ReceiveInformation;
            PhysicSystem.OnUpdateData -= LogicSystem.ReceiveInformation;

            LogicSystem.Event.SetVelocity -= PhysicSystem.SetVelocity;
            LogicSystem.Event.SetRotation -= PhysicSystem.SetRotation;
            LogicSystem.Event.SetPhysicModuleActive -= PhysicModule.SetActive;

            LogicSystem.Event.SetSmoothRotation -= PhysicSystem.SetSmoothRotation;
            LogicSystem.Event.SetBool_Anim -= AnimModule.SetBool;
            LogicSystem.Event.SetFloat_Anim -= AnimModule.SetFloat;
            LogicSystem.Event.SetInt_Anim -= AnimModule.SetInt;
            LogicSystem.Event.DealDamage -= DealDamage;
            LogicSystem.Event.SpecialDealDamage -= DealDamage;
            LogicSystem.Event.CollideGift -= OnCollideGift;
            LogicSystem.Event.EndGiftBonus -= OnEndGiftBonus;

            AnimModule.UpdateEventAnimationState -= LogicSystem.ReceiveInformation;
            #endregion
        }
        public virtual void Reset(bool isRpcCall = false)
        {
            SetLevel(1);
            SetPosition(new Vector3(0, GAMECONST.INIT_CHARACTER_HEIGHT, 0));
            OnInit();
            Data.Hp = 1;
        }

        public void SetPosition(Vector3 position)
        {
            PhysicModule.SetActive(false);
            transform.localPosition = position;
            PhysicModule.SetActive(true);
        }

        public void SetLevel(int level)
        {
            Data.Level = level;
            PhysicModule.SetScale(GAMECONST.PHYSIC_OTYPE.Character, Vector3.one * Data.Size);
        }
        protected virtual void Update()
        {
            NavigationSystem.Run();
            LogicSystem.Run();
            PhysicSystem.Run();
        }

        protected virtual void FixedUpdate()
        {
            WorldInterfaceSystem.Run();
            LogicSystem.FixedUpdateData();
        }

        protected virtual void DealDamage(Vector3 direction, float range)
        {
            Weapon.DealDamage(direction, range ,Data.Size,false ,Data.Speed / CharacterData.BASE_SPEED);
        }

        protected virtual void DealDamage(Vector3 direction, float range, bool isSpecial)
        {
            Weapon.DealDamage(direction, range, Data.Size, isSpecial, Data.Speed / CharacterData.BASE_SPEED);
        }

        public virtual void ChangeColor(GameColor color)
        {
            Material mat = GameplayManager.Inst.GetMaterial(color);
            meshCharacter.material = mat;
            Data.Color = (int)color;
            VFX_Hit.SetColor(GameplayManager.Inst.GetColor(Color));
        }

        public virtual void ChangePant(PantSkin name)
        {
            Material mat = GameplayManager.Inst.GetMaterial(name);
            meshPant.material = mat;
            Data.Pant = (int)name;
        }
        public virtual void ChangeHair(PoolID hair)
        {
            if(hair != PoolID.None)
            {
                GameObject hairObject = PrefabManager.Inst.PopFromPool(hair);
                hairObject.transform.parent = ContainHairTF;
                Cache.GetItem(hairObject).SetTranformData();

                if(this.hair != null)
                {
                    Cache.GetItem(this.hair).OnDespawn();
                }
                this.hair = hairObject;
            }
            Data.Hair = (int)hair;
        }

        public virtual void ChangeHair(GameObject hairObject)
        {
            hairObject.transform.parent = ContainHairTF;
            Cache.GetItem(hairObject).SetTranformData();

            if (this.hair != null)
            {
                Cache.GetItem(this.hair).OnDespawn();
            }
            this.hair = hairObject;
        }
        public virtual void ChangeWeapon(BaseWeapon weapon)
        {
            if(weapon != null)
            {
                if(Weapon != null)
                {
                    Weapon.OnDespawn();
                }
                
                Weapon = weapon;
                Weapon.Character = this;
                Weapon.gameObject.transform.parent = ContainWeaponTF;
                Weapon.SetTranformData();
                Data.Weapon = (int)Weapon.Name;
            }          
        }      
        protected void CalculateActionAndTime()
        {
            #region DIE EVENTS
            dieTimes = new List<float>() { GAMECONST.ANIM_IS_DEAD_TIME, GAMECONST.ANIM_IS_DEAD_TIME + despawnTime };
            dieActions = new List<Action>() { () => OnDie?.Invoke(this), OnDespawn };
            #endregion
        }

        public virtual void TakeDamage(int damage, bool isRpcCall = false)
        {
            Data.Hp -= damage;
            VFX_Hit?.Play();
            SoundManager.Inst.PlaySound(SoundManager.Sound.Character_Hit,transform.position);
            if(Data.Hp <= 0)
            {
                SoundManager.Inst.PlaySound(SoundManager.Inst.GetRandomDieSound(), transform.position);
                dieTimer.Start(dieTimes, dieActions);
            }
        }

        //TODO: Combat Function(Covert to a system)
        public virtual void AddStatus(bool isRpcCall = false)
        {
            //TODO: Increase Size of character
            //TODO: Increase Size of Attack Range Indicator
            Data.Level += 1;
            SoundManager.Inst.PlaySound(SoundManager.Sound.Character_SizeUp, transform.position);           
            VFX_AddStatus?.Play();
            PhysicModule.SetScale(GAMECONST.PHYSIC_OTYPE.Character, Vector3.one * Data.Size);          
        }

        protected virtual void OnCollideGift(Collider col)
        {
            if (col)
            {
                Data.BaseAttackRange = CharacterData.BASE_ATTACK_RANGE * GIFT_BONUS;
                Cache.GetGift(col.gameObject).OnDespawn();
            }
        }

        protected virtual void OnEndGiftBonus()
        {
            Data.BaseAttackRange = CharacterData.BASE_ATTACK_RANGE;
        }

             
    }
}

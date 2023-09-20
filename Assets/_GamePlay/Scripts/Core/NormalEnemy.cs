using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.Core
{
    using MoveStopMove.Manager;
    using MoveStopMove.Core.Character.LogicSystem;
    using MoveStopMove.Core.Character.NavigationSystem;
    using MoveStopMove.Core.Data;
    using Utilitys.Timer;
    using MoveStopMove.ContentCreation.Weapon;

    public class NormalEnemy : BaseCharacter
    {
        static int index = 0;
        [SerializeField]
        PhotonCharacter photon;
        protected override void Awake()
        {           
            base.Awake();
            LogicSystem.SetCharacterInformation(Data, gameObject.transform);
            WorldInterfaceSystem.SetCharacterInformation(Data);

            switch (GameplayManager.Inst.GameMode)
            {
                case GAMECONST.GAMEPLAY_MODE.STANDARD_PVP:
                    photon._OnInitialize += Initialize;
                    photon._OnAddDamage += TakeDamage;
                    photon._OnAddStatus += AddStatus;
                    photon._OnReset += Reset;
                    if (!photon.photonView.IsMine)
                    {
                        photon._OnInitData += LoadData;
                        photon._OnUpdateCharacter += UpdateCharacter;
                    }
                    break;
            }
        }

        public void Initialize()
        {
            gameObject.name = "Enemy" + index;
            index++;
            VFXInit();
            OnInit();           
        }

        public override void OnInit()
        {
            base.OnInit();
            Data.Hp = 1;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            //Debug.Log($"<color=green> On Spawn </color>");
            switch (GameplayManager.Inst.GameMode)
            {
                case GAMECONST.GAMEPLAY_MODE.STANDARD_PVE:
                    VFXInit();
                    break;
            }
        }        
        protected override void OnDisable()
        {
            base.OnDisable();
            dieTimer.Stop();
            //Debug.Log($"<color=yellow> On Disable </color>");
        }
        
        public override void OnDespawn()
        {
            base.OnDespawn();
            //Debug.Log($"<color=red> On Despawn </color>");
            VFXRecalling();
            ((CharacterLogicModule)LogicModule).StopStateMachine();
            switch (GameplayManager.Inst.GameMode)
            {
                case GAMECONST.GAMEPLAY_MODE.STANDARD_PVE:
                    PrefabManager.Inst.PushToPool(this.gameObject, PoolID.Character);
                    VFX_AddStatus = null;
                    VFX_Hit = null;
                    break;
                case GAMECONST.GAMEPLAY_MODE.STANDARD_PVP:
                    if (photon && !photon.photonView.IsMine)
                        ((CanvasGameplay)UIManager.Inst.GetUI(UIID.UICGamePlay)).UnsubcribeTarget(this);
                    if (NetworkManager.Inst.IsMasterClient)
                        NetworkManager.Inst.Destroy(gameObject);
                    break;
            }

            
        }
        private void VFXInit()
        {
            VFX_Hit = Cache.GetVisualEffectController(VisualEffectManager.Inst.PopFromPool(VisualEffect.VFX_Hit));
            VFX_AddStatus = Cache.GetVisualEffectController(VisualEffectManager.Inst.PopFromPool(VisualEffect.VFX_AddStatus));
            VFX_Hit.Init(transform, Vector3.up * 0.5f, Quaternion.Euler(Vector3.zero), Vector3.one * 0.3f);
            VFX_AddStatus.Init(transform, Vector3.up * -0.5f, Quaternion.Euler(-90, 0, 0), Vector3.one);
        }
        private void VFXRecalling()
        {
            if (VFX_Hit)
                VisualEffectManager.Inst.PushToPool(VFX_Hit.gameObject, VisualEffect.VFX_Hit);
            if (VFX_AddStatus)
                VisualEffectManager.Inst.PushToPool(VFX_AddStatus.gameObject, VisualEffect.VFX_AddStatus);
        }

        public override void Run()
        {          
            ((CharacterAI)NavigationModule).StartStateMachine();           
        }

        public override void Stop()
        {
            ((CharacterAI)NavigationModule).StopStateMachine();
        }

        public override void Reset(bool isRpcCall = false)
        {
            base.Reset(isRpcCall);
            if (photon && !isRpcCall)
            {
                photon.UpdateNetworkEvent(PhotonCharacter.EVENT.REVIVE);
            }
        }
        public override void TakeDamage(int damage, bool isRpcCall = false)
        {
            base.TakeDamage(damage, isRpcCall);
            if (photon && !isRpcCall)
            {
                photon.UpdateNetworkEvent(PhotonCharacter.EVENT.TAKE_DAMAGE, new object[1] { 1 });
            }
        }
        public override void AddStatus(bool isRpcCall = false)
        {
            base.AddStatus();
            if (photon && !isRpcCall)
            {
                photon.UpdateNetworkEvent(PhotonCharacter.EVENT.LEVEL_UP);
            }
        }
        public void SetUpCharacter(GameColor color, PoolID hair, PantSkin pant, PoolID weapon, int level)
        {         
            Data.Weapon = (int)weapon;
            Data.Color = (int)color;
            Data.Hair = (int)hair;
            Data.Pant = (int)pant;
            SetLevel(level);

            int[] data = new int[8];
            data[2] = Data.Weapon;
            data[3] = Data.Color;
            data[4] = Data.Hair;
            data[5] = Data.Pant;
            data[6] = Data.Set;
            data[7] = level;
            GameObject newHair = PrefabManager.Inst.PopFromPool(hair);
            GameObject newWeapon = PrefabManager.Inst.PopFromPool(weapon);
            UpdateCharacter(newWeapon, newHair);
            photon.SetNetworkData(newWeapon, newHair, ref data);
        }
        private void LoadData(int[] data)
        {
            Data.Weapon = data[0];
            Data.Color = data[1];
            Data.Hair = data[2];
            Data.Pant = data[3];
            Data.Set = data[4];
        }

        private void UpdateCharacter(GameObject newWeapon, GameObject hairObject = null, int level = 1, bool isMine = true)
        {
            ChangeColor((GameColor)Data.Color);
            if (hairObject == null)
                ChangeHair((PoolID)Data.Hair);
            else
                ChangeHair(hairObject);
            ChangePant((PantSkin)Data.Pant);

            if (isMine)
                ChangeWeapon(Cache.GetBaseWeapon(newWeapon)); //Has Save Data
            else
                base.ChangeWeapon(Cache.GetBaseWeapon(newWeapon)); //Not Have Save Data
            SetLevel(level);
            if (photon && !photon.photonView.IsMine)
                ((CanvasGameplay)UIManager.Inst.GetUI(UIID.UICGamePlay)).SubscribeTarget(this);
        }
    }
}
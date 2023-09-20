using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.Core
{
    using MoveStopMove.ContentCreation.Weapon;
    using MoveStopMove.Core.Character.NavigationSystem;
    using MoveStopMove.Core.Data;
    using MoveStopMove.Manager;
    using MoveStopMove.Core.Character.WorldInterfaceSystem;
    public class Player : BaseCharacter
    {
        public const string P_SPEED = "PlayerSpeed";
        public const string P_WEAPON = "PlayerWeapon";

        public const string P_COLOR = "PlayerColor";
        public const string P_HAIR = "PlayerHair";
        public const string P_PANT = "PlayerPant";       
        public const string P_SET = "PlayerSet";

        public const string P_HIGHTEST_SCORE = "PlayerHighestScore";
        public const string P_CURRENT_REGION = "PlayerCurrentRegion";
        public const string P_CASH = "PlayerCash";
        

        [SerializeField]
        private AttackIndicator attackIndicator;
        [SerializeField]
        private PhotonCharacter photon;

        private GameData GameData;
        private bool undying = false;
        protected override void Awake()
        {
            base.Awake();
            LogicSystem.SetCharacterInformation(Data, gameObject.transform);
            WorldInterfaceSystem.SetCharacterInformation(Data);
            GameData = GameManager.Inst.GameData;
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

        protected override void OnEnable()
        {
            base.OnEnable();
            LogicSystem.Event.SetTargetIndicatorPosition += SetIndicatorPosition;
            
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            LogicSystem.Event.SetTargetIndicatorPosition -= SetIndicatorPosition;
        }

        protected void Start()
        {
            switch (GameplayManager.Inst.GameMode)
            {
                case GAMECONST.GAMEPLAY_MODE.STANDARD_PVE:
                    Initialize();
                    CanvasTest._OnUndying += TestUndying;
                    break;
            }   
        }

        public void Initialize()
        {
            VFX_Hit = Cache.GetVisualEffectController(VisualEffectManager.Inst.PopFromPool(VisualEffect.VFX_Hit));
            VFX_AddStatus = Cache.GetVisualEffectController(VisualEffectManager.Inst.PopFromPool(VisualEffect.VFX_AddStatus));
            VFX_Hit.Init(transform, Vector3.up * 0.5f, Quaternion.Euler(Vector3.zero), Vector3.one * 0.3f);
            VFX_AddStatus.Init(transform, Vector3.up * -0.5f, Quaternion.Euler(-90, 0, 0), Vector3.one);   
            LoadData();
            if(photon && !photon.photonView.IsMine)
                ((CanvasGameplay)UIManager.Inst.GetUI(UIID.UICGamePlay)).SubscribeTarget(this);
        }

        public override void OnInit()
        {
            base.OnInit();
            Data.Hp = 1;
            ((InputModule)NavigationModule).Active = true;
            attackIndicator.ScaleUp(1);
        }

        public override void OnDespawn()
        {
            
        }

        public override void ChangeColor(GameColor color)
        {
            base.ChangeColor(color);
            GameData.SetIntData(P_COLOR, ref GameData.Color, (int)color);
        }

        public override void ChangeHair(PoolID hair)
        {
            base.ChangeHair(hair);
            GameData.SetIntData(P_HAIR, ref GameData.Hair, (int)hair);
        }

        public override void ChangePant(PantSkin name)
        {
            base.ChangePant(name);
            GameData.SetIntData(P_PANT, ref GameData.Pant, (int)name);
        }

        public override void ChangeWeapon(BaseWeapon weapon)
        {
            base.ChangeWeapon(weapon);
            GameData.SetIntData(P_WEAPON, ref GameData.Weapon, (int)weapon.Name);
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
            if (undying) return;
            base.TakeDamage(damage, isRpcCall);
            if (photon && !isRpcCall)
            {
                photon.UpdateNetworkEvent(PhotonCharacter.EVENT.TAKE_DAMAGE, new object[1] { 1 });
            }
            if (photon && !photon.photonView.IsMine)
                ((CanvasGameplay)UIManager.Inst.GetUI(UIID.UICGamePlay)).UnsubcribeTarget(this);
        }
        public override void AddStatus(bool isRpcCall = false)
        {
            base.AddStatus();
            SetCameraPosition(Data.Size);

            if (photon && !isRpcCall)
            {
                photon.UpdateNetworkEvent(PhotonCharacter.EVENT.LEVEL_UP);
                UIManager.Inst.GetUI<CanvasGameplay>(UIID.UICGamePlay).AddLocalPlayerScore(1);
            }
        }

        public void Win()
        {
            ((InputModule)NavigationModule).Active = false;
        }
        protected override void OnCollideGift(Collider col)
        {
            base.OnCollideGift(col);
            if (col)
            {
                attackIndicator.ScaleUp(GIFT_BONUS);
                SetCameraPosition(Data.Size * GIFT_BONUS);
            }           
        }

        protected override void OnEndGiftBonus()
        {
            base.OnEndGiftBonus();
            attackIndicator.ScaleUp(1);
            SetCameraPosition(Data.Size);
        }

        private void SetCameraPosition(float value)
        {
            switch (GameplayManager.Inst.GameMode)
            {
                case GAMECONST.GAMEPLAY_MODE.STANDARD_PVE:
                    GameplayManager.Inst.SetCameraPosition(value);
                    break;
                case GAMECONST.GAMEPLAY_MODE.STANDARD_PVP:
                    if (photon.photonView.IsMine)
                        GameplayManager.Inst.SetCameraPosition(value);
                    break;
            }
        }

        private void SetIndicatorPosition(BaseCharacter character, bool active)
        {
            
            GameplayManager.Inst.TargetIndicator.SetActive(active);
            if (!active) return;

            GameplayManager.Inst.TargetIndicator.transform.position = character.gameObject.transform.position + Vector3.up * 0.1f;
            GameplayManager.Inst.TargetIndicator.transform.localScale = character.Size * GameplayManager.Inst.InitScaleTargetIndicator;
        }


        private void LoadData()
        {
            GameObject newWeapon;
            GameObject newHair;
            //Data.Speed = GameData.Speed;
            switch (GameplayManager.Inst.GameMode)
            {
                case GAMECONST.GAMEPLAY_MODE.STANDARD_PVE:
                    Data.Weapon = GameData.Weapon;
                    Data.Color = GameData.Color;
                    Data.Hair = GameData.Hair;
                    Data.Pant = GameData.Pant;
                    Data.Set = GameData.Set;
                    newWeapon = PrefabManager.Inst.PopFromPool((PoolID)Data.Weapon);
                    UpdateCharacter(newWeapon);
                    break;
                case GAMECONST.GAMEPLAY_MODE.STANDARD_PVP:
                    if (photon.photonView.IsMine)
                    {
                        int[] data = new int[8];
                        Data.Weapon = GameData.Weapon;
                        data[2] = GameData.Weapon;
                        Data.Color = GameData.Color;
                        data[3] = GameData.Color;
                        Data.Hair = GameData.Hair;
                        data[4] = GameData.Hair;
                        Data.Pant = GameData.Pant;
                        data[5] = GameData.Pant;
                        Data.Set = GameData.Set;
                        data[6] = GameData.Set;
                        data[7] = Data.Level;
                        newWeapon = PrefabManager.Inst.PopFromPool((PoolID)Data.Weapon);
                        newHair = PrefabManager.Inst.PopFromPool((PoolID)Data.Hair);
                        UpdateCharacter(newWeapon, newHair);
                        photon.SetNetworkData(newWeapon,newHair,ref data);
                    }
                    break;
            }         
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
        }

        private void OnDestroy()
        {
            switch (GameplayManager.Inst.GameMode)
            {
                case GAMECONST.GAMEPLAY_MODE.STANDARD_PVE:
                    CanvasTest._OnUndying -= TestUndying;
                    break;

            }
        }

        private void TestUndying(bool value)
        {
            undying = value;
        }
    }
}
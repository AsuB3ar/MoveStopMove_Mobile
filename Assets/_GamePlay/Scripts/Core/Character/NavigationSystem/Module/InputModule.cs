using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.Core.Character.NavigationSystem
{
    using MoveStopMove.Manager;
    using Photon.Pun;
    using Utilitys.Input;
    public class InputModule : AbstractNavigationModule
    {
        JoyStick joyStick;
        Vector2 moveDirection = Vector2.zero;
        private bool active = false;
        [SerializeField]
        PhotonView photonView;
        public bool Active
        {
            set
            {
                active = value;
                if (active == false)
                {
                    moveDirection = Vector2.zero;
                    Data.MoveDirection = Vector3.zero;
                }
            }
        }

        private void Start()
        {
            CanvasGameplay gameplay;
            switch (GameplayManager.Inst.GameMode)
            {
                case GAMECONST.GAMEPLAY_MODE.STANDARD_PVE:
                    gameplay = (CanvasGameplay)UIManager.Inst.GetUI(UIID.UICGamePlay);
                    joyStick = gameplay.joyStick;
                    joyStick.OnMove += UpdateMoveDirection;
                    gameplay.Close();
                    break;
                case GAMECONST.GAMEPLAY_MODE.STANDARD_PVP:
                    if (photonView.IsMine)
                    {
                        gameplay = (CanvasGameplay)UIManager.Inst.GetUI(UIID.UICGamePlay);
                        joyStick = gameplay.joyStick;
                        joyStick.OnMove += UpdateMoveDirection;
                    }
                    break;
            }
            
        }
        public override void UpdateData()
        {
            Vector3 move = (Vector3.right * moveDirection.x + Vector3.forward * moveDirection.y).normalized;
            Data.MoveDirection = move;
        }


        private void UpdateMoveDirection(Vector2 moveDirection)
        {
            this.moveDirection = moveDirection;
        }

        private void OnDestroy()
        {
            if (joyStick == null) return;
            joyStick.OnMove -= UpdateMoveDirection;
        }
    }
}
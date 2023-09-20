using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilitys.AI
{
    using MoveStopMove.Core.Character.LogicSystem;
    public class MoveState : BaseState<LogicParameter,LogicData>
    {
        public MoveState(StateMachine<LogicParameter, LogicData> StateMachine, LogicParameter Parameter, LogicData Data, LogicEvent Event) 
            : base(StateMachine ,Parameter, Data, Event)
        {

        }
        public override void Enter()
        {
            base.Enter();
            Event.SetBool_Anim(GAMECONST.ANIM_IS_IDLE, false);
            Data.CharacterData.AttackCount = 1;
            //TODO: Play Move Animation
            //TODO: Set up Timer
        }


        public override void EventUpdate(Type type, string code)
        {
            base.EventUpdate(type, code);
        }



        public override int LogicUpdate()
        {
            if (Data.CharacterData.Hp <= 0)
            {
                StateMachine.ChangeState(State.Die);
                return -1;
            }
            else if(Parameter.MoveDirection.sqrMagnitude < 0.001)
            {
                StateMachine.ChangeState(State.Idle);
                return 0;
            }


            RotationHandle();
            
            
            
            return 0;
        }
        public override int PhysicUpdate()
        {
            MovementHandle();
            return 0;
        }
        private void MovementHandle()
        {
            if(Parameter.IsHaveGround && Parameter.IsGrounded)
            {
                Event.SetVelocity(Parameter.MoveDirection * Data.CharacterData.Speed * Time.fixedDeltaTime * 60);
            }
            else if(!Parameter.IsHaveGround && Parameter.IsGrounded)
            {
                Event.SetVelocity(-Parameter.PlayerTF.forward * Time.deltaTime);
            }
            
        }

        private void RotationHandle()
        {
            Event.SetSmoothRotation(GAMECONST.PHYSIC_OTYPE.Model, Parameter.MoveDirection);
            Event.SetSmoothRotation(GAMECONST.PHYSIC_OTYPE.Sensor, Parameter.MoveDirection);
        }
        

    }
}
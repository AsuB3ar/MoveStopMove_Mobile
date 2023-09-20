using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilitys.AI
{
    using MoveStopMove.Core.Character.LogicSystem;
    public class IdleState : BaseState<LogicParameter,LogicData>
    {
        public IdleState(StateMachine<LogicParameter,LogicData> StateMachine,LogicParameter Parameter, LogicData Data, LogicEvent Event) 
            : base(StateMachine, Parameter, Data, Event)
        {
           
        }
        public override void Enter()
        {
            base.Enter();
            //TODO: Play Animation 
            Event.SetVelocity(Vector3.zero);           
            Event.SetBool_Anim(GAMECONST.ANIM_IS_IDLE, true);
        }

        public override void Exit()
        {
            base.Exit();
        }


        public override int LogicUpdate()
        {
            base.LogicUpdate();
            if (Data.CharacterData.Hp <= 0)
            {
                StateMachine.ChangeState(State.Die);
            }
            else if (Parameter.TargetCharacter != null && Data.CharacterData.AttackCount > 0)
            {
                StateMachine.ChangeState(State.Attack);
            }
            else if(Parameter.MoveDirection.sqrMagnitude > 0.001)
            {
                StateMachine.ChangeState(State.Move);
            }
            return 0;
        }


    }
}
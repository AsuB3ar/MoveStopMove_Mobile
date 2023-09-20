using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilitys.AI
{
    using MoveStopMove.Core.Character.LogicSystem;
    using Utilitys.Timer;

    public class DieState : BaseState<LogicParameter,LogicData>
    {
        public DieState(StateMachine<LogicParameter,LogicData> StateMachine, LogicParameter Parameter, LogicData Data, LogicEvent Event) 
            : base(StateMachine,Parameter, Data, Event)
        {

        }
        public override void Enter()
        {
            Event.SetVelocity(Vector3.zero);
            Event.SetBool_Anim(GAMECONST.ANIM_IS_DEAD, true);
            Event.SetPhysicModuleActive(false);
            base.Enter();
        }


        public override void EventUpdate(Type type, string code)
        {
            base.EventUpdate(type, code);
        }

        public override void Exit()
        {
            base.Exit();
            Event.SetBool_Anim(GAMECONST.ANIM_IS_DEAD, false);
            Event.SetPhysicModuleActive(true);
        }

    }
}
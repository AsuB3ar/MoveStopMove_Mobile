using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilitys.AI
{
    using System;
    using MoveStopMove.Core.Character.LogicSystem;
    using MoveStopMove.Core.Character;

    public abstract class BaseState<P,D>
        where P : AbstractParameterSystem
        where D : AbstractDataSystem<D>
    {
        protected StateMachine<P,D> StateMachine;
        protected P Parameter;
        protected D Data;
        protected LogicEvent Event;
        protected float StartTime { get; private set; }
        public string AnimBoolName { get; private set; }

        protected bool isEndState;
        //protected Rigidbody2D Rb;
        public BaseState(StateMachine<P,D> StateMachine, P Parameter, D Data, LogicEvent Event)
        {
            this.StateMachine = StateMachine;
            this.Parameter = Parameter;
            this.Data = Data;
            this.Event = Event;
            //Rb = player.MovementModule.Rb;
        }      
        public virtual void Enter()
        {
            StartTime = Time.time;
            isEndState = false;       
        }
        public virtual void Exit()
        {
            isEndState = true;
        }
        public virtual int LogicUpdate()
        { 
            return 0;
        }
        public virtual int PhysicUpdate()
        {
            return 0;
        }
        public virtual void EventUpdate(Type type,string code)
        {

        }
    }
}
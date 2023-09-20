
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.Core.Character.LogicSystem
{
    using Utilitys.AI;
    public class CharacterLogicModule : AbstractLogicModule
    {
        public readonly List<State> StateName = new List<State>() { State.Idle, State.Move, State.Attack, State.Die };
        public StateMachine<LogicParameter,LogicData> StateMachine;
        BasicStateInsts<LogicParameter,LogicData> States;

        public override void Initialize(LogicData Data, LogicParameter Parameter, LogicEvent Event)
        {
            base.Initialize(Data, Parameter, Event);
            StateMachine = new StateMachine<LogicParameter,LogicData>();

            StateMachine.PushState(State.Idle,new IdleState(StateMachine, Parameter, Data, Event));
            StateMachine.PushState(State.Move,new MoveState(StateMachine, Parameter, Data, Event));
            StateMachine.PushState(State.Attack,new AttackState(StateMachine, Parameter, Data, Event));
            StateMachine.PushState(State.Die,new DieState(StateMachine, Parameter, Data, Event));

            //StateMachine.Report = true;            
        }

        bool isIndicatorActive = false;
        public override void UpdateData()
        {
            if (StateMachine.IsStarted)
            {
                StateMachine.CurrentState.LogicUpdate();

                if(Parameter.TargetCharacter != null)
                {
                    Event.SetTargetIndicatorPosition?.Invoke(Parameter.TargetCharacter, true);
                    isIndicatorActive = true;
                }
                else if(isIndicatorActive)
                {
                    Event.SetTargetIndicatorPosition?.Invoke(null, false);
                    isIndicatorActive = false;
                }

            }
            
        }

        public override void FixedUpdateData()
        {
            if (StateMachine.IsStarted)
            {
                StateMachine.CurrentState.PhysicUpdate();
            }
        }

        public void StartStateMachine()
        {
            StateMachine.Start(State.Idle);

            bool value = Parameter.Gift; //TODO: Need to change
        }

        public void StopStateMachine()
        {
            StateMachine.Stop();
        }
    }
}
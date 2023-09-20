using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.Core.Character.NavigationSystem
{
    using Utilitys.AI;
    public class CharacterAI : AbstractNavigationModule
    {
        public readonly List<State> StateName = new List<State>() { State.Wandering, State.Combat };
        public StateMachine<NavigationParameter,NavigationData> StateMachine;
        public override void Initialize(NavigationData Data, NavigationParameter Parameter)
        {
            base.Initialize(Data, Parameter);
            StateMachine = new StateMachine<NavigationParameter,NavigationData>();

            StateMachine.PushState(State.Wandering, new NavWanderingState(StateMachine, Parameter, Data));
            StateMachine.PushState(State.Combat, new NavCombatState(StateMachine, Parameter, Data));
        }
        public override void UpdateData()
        {
            if (StateMachine.IsStarted)
            {
                StateMachine.CurrentState.LogicUpdate();
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
            StateMachine.Start(State.Wandering);
        }

        public void StopStateMachine()
        {
            StateMachine.Stop();
            Data.MoveDirection = Vector3.zero;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilitys.AI
{
    using MoveStopMove.Core.Character;
    public enum State
    {
        Idle = 0,
        Move = 1,
        Jump = 2,
        Die = 3,
        Attack = 4,
        Wandering = 100,
        Combat = 101,
    }
   
    public class StateMachine<P,D>
        where P : AbstractParameterSystem
        where D : AbstractDataSystem<D>
    {
        Dictionary<State, BaseState<P, D>> states = new Dictionary<State, BaseState<P, D>>();
        //DEVELOP:Change condition to "If Name = null -> State = null"
        public BaseState<P,D> CurrentState { get; private set; }
        public bool IsStarted { get; private set; } = false; 
        public bool Report = false;
        public void Start(BaseState<P,D> initState)
        {
            if (CurrentState != null) CurrentState.Exit();

            CurrentState = initState;
            CurrentState.Enter();
            IsStarted = true;
        }

        public void Start(State state)
        {
            Start(states[state]);
        }

        public void Stop()
        {
            if (CurrentState != null) CurrentState.Exit();
            CurrentState = null;
            IsStarted = false;
        }
        public void ChangeState(BaseState<P, D> newState)
        {
            if (newState != null)
            {
                if (Report)
                {
                    Debug.Log("Change to" + newState.ToString());
                }
                
                CurrentState.Exit();
                CurrentState = newState;
                CurrentState.Enter();
            }
            else
            {
                Debug.LogError("NUll STATE");
            }
        }

        public void ChangeState(State state)
        {
            if (states.ContainsKey(state))
            {
                ChangeState(states[state]);
            }
            else
            {
                Debug.LogError("NUll STATE");
            }
        }
       
        public BaseState<P, D> GetState(State name)
        {
            return states[name];
        }

        public void PushState(State state, BaseState<P, D> stateScript)
        {
            if (states.ContainsKey(state))
            {
                return;
            }
            else
            {
                states.Add(state, stateScript);
            }
        }

    }
}

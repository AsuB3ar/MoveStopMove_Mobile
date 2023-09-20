using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilitys.AI
{
    using MoveStopMove.Core.Character.NavigationSystem;
    using Utilitys.Timer;
    using MoveStopMove.Manager;
    public class NavWanderingState : BaseState<NavigationParameter,NavigationData>
    {
        STimer timer;
        float timeWandering;
        float avgTimeWandering = 2f;
        public NavWanderingState(StateMachine<NavigationParameter, NavigationData> StateMachine, NavigationParameter Parameter, NavigationData Data)
            :base(StateMachine,Parameter,Data,null)
        {
            timer = TimerManager.Inst.PopSTimer();
        }

        public override void Enter()
        {
            base.Enter();
            ChangeRandomDirection();
            timeWandering = Random.Range(avgTimeWandering * 0.8f, avgTimeWandering * 1.2f);            
            timer.Start(timeWandering, ChangeRandomDirection);
        }
        public override int LogicUpdate()
        {
            if(Parameter.TargetCharacter != null)
            {
                StateMachine.ChangeState(State.Combat);
            }
            else if(Parameter.IsGrounded && !Parameter.IsHaveGround)
            {
                WhenMeetWall();
            }
            else if (Parameter.IsHaveObstances)
            {
                WhenMeetObstance();
            }
            return base.LogicUpdate();
        }
        public override void Exit()
        {
            base.Exit();
            timer.Stop();
        }
        private void WhenMeetObstance()
        {
            float value = Parameter.SensorTF.localRotation.eulerAngles.y + 90;
            Vector2 newDirection = MathHelper.GetRandomDirection(value - 90, value + 90);

            Data.MoveDirection = new Vector3(newDirection.x, 0, newDirection.y);
        }
        private void WhenMeetWall()
        {
            float value = Parameter.SensorTF.localRotation.eulerAngles.y - 90;
            Vector2 newDirection = MathHelper.GetRandomDirection(value - 90, value + 90);

            Data.MoveDirection = new Vector3(newDirection.x, 0, newDirection.y);
        }
        private void ChangeRandomDirection()
        {
            int value = Random.Range(0, 5);
            Vector2 newDirection;
            if (value < 3.5)
            {
                Vector3 direction = MathHelper.GetRandomDirection();
                float angle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up) + 90;
                newDirection = MathHelper.GetRandomDirection(angle - 45,angle + 45);
            }
            else
            {
                newDirection = Vector2.zero;
            }
             
            Data.MoveDirection = new Vector3(newDirection.x, 0, newDirection.y);
            timeWandering = Random.Range(avgTimeWandering * 0.8f, avgTimeWandering * 1.2f);
            timer.Start(timeWandering, ChangeRandomDirection);
        }
        ~NavWanderingState()
        {
            TimerManager.Inst.PushSTimer(timer);
        }
    }
}
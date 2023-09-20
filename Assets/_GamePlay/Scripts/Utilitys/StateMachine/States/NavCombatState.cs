using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilitys.AI
{
    using MoveStopMove.Core.Character.NavigationSystem;
    using Utilitys.Timer;
    public class NavCombatState : BaseState<NavigationParameter, NavigationData>
    {
        STimer timerEndState;
        STimer timerHitAndRun;

        float avgTimeHitAndRun = 0.2f;
        float avgTimeEndState = 3;

        Vector3 direction = new Vector3();
        bool isCanAttack = true;
        public NavCombatState(StateMachine<NavigationParameter, NavigationData> StateMachine, NavigationParameter Parameter, NavigationData Data) 
            : base(StateMachine, Parameter, Data, null)
        {
            timerEndState = TimerManager.Inst.PopSTimer();
            timerHitAndRun = TimerManager.Inst.PopSTimer();
            timerEndState.TimeOut += TimerEvent;
            timerHitAndRun.TimeOut += TimerEvent;
        }

        public override void Enter()
        {
            base.Enter();
            float time = Random.Range(avgTimeHitAndRun * 0.8f, avgTimeHitAndRun * 1.2f);
            timerHitAndRun.Start(time, 1);

        }

        public override int LogicUpdate()
        {
            if(Parameter.TargetCharacter == null)
            {
                if (!timerEndState.IsStart)
                {
                    NotHaveCharacterInRange();
                }
            }
            else
            {
                timerEndState.Stop();
            }

            if (Parameter.IsGrounded && !Parameter.IsHaveGround)
            {
                WhenMeetWall();
            }
            else if (Parameter.IsHaveObstances)
            {
                WhenMeetObstance();
            }
            Data.MoveDirection = direction;
            return base.LogicUpdate();
        }

        public void HitAndRun()
        {
            if (isCanAttack)
            {
                direction.Set(0, 0, 0);
            }
            else
            {
                Vector2 dir = MathHelper.GetRandomDirection();
                direction.Set(dir.x, 0, dir.y);
            }
            isCanAttack = !isCanAttack;

            float time = Random.Range(GAMECONST.ANIM_IS_ATTACK_TIME + avgTimeHitAndRun * 0.8f,GAMECONST.ANIM_IS_ATTACK_TIME + avgTimeHitAndRun * 1.2f);
            timerHitAndRun.Start(time, 1);
        }

        public void NotHaveCharacterInRange()
        {
            float time = Random.Range(avgTimeEndState * 0.8f, avgTimeEndState * 1.2f);
            timerEndState.Start(time, 0);
        }

        public void TimerEvent(int code)
        {
            if(code == 0)
            {
                StateMachine.ChangeState(State.Wandering);
            }
            else if(code == 1)
            {
                HitAndRun();
            }
        }

        public override void Exit()
        {
            base.Exit();
            timerEndState.Stop();
            timerHitAndRun.Stop();
        }

        private void WhenMeetObstance()
        {
            float value = Parameter.SensorTF.localRotation.eulerAngles.y + 90;
            Vector2 newDirection = MathHelper.GetRandomDirection(value - 90, value + 90);
            direction.Set(newDirection.x, 0, newDirection.y);
        }

        private void WhenMeetWall()
        {
            float value = Parameter.SensorTF.localRotation.eulerAngles.y - 90;
            Vector2 newDirection = MathHelper.GetRandomDirection(value - 90, value + 90);
            direction.Set(newDirection.x, 0, newDirection.y);
        }


        ~NavCombatState()
        {
            timerEndState.TimeOut -= TimerEvent;
            timerHitAndRun.TimeOut -= TimerEvent;
        }

    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilitys.AI
{
    using MoveStopMove.Core.Character.LogicSystem;
    public class AttackState : BaseState<LogicParameter,LogicData>
    {
        private int timeFrames;
        Vector3 direction;
        Vector3 targetPosition;

        bool IsSpecialAttack;
        public AttackState(StateMachine<LogicParameter,LogicData> StateMachine, LogicParameter Parameter, LogicData Data, LogicEvent Event) 
            : base(StateMachine ,Parameter, Data, Event)
        {

        }
        public override void Enter()
        {   
            base.Enter();
            IsSpecialAttack = Parameter.Gift;

            if (IsSpecialAttack == false)
            {
                Event.SetBool_Anim(GAMECONST.ANIM_IS_ATTACK, true);
            }
            else
            {
                Event.SetBool_Anim(GAMECONST.ANIM_IS_ULTI, true);
            }

            Event.SetVelocity(Vector3.zero);

            targetPosition = Parameter.TargetCharacter.gameObject.transform.position;
            direction = Parameter.TargetCharacter.gameObject.transform.position - Parameter.PlayerTF.position;

            Quaternion rot = MathHelper.GetQuaternion2Vector(Vector2.up, new Vector2(-direction.x, direction.z));

            timeFrames = 0;
            Event.SetRotation(GAMECONST.PHYSIC_OTYPE.Model, rot);
            Event.SetRotation(GAMECONST.PHYSIC_OTYPE.Sensor, rot);
            Data.CharacterData.AttackCount = 0;

        }

        public override int LogicUpdate()
        {
            if (Data.CharacterData.Hp <= 0)
            {
                StateMachine.ChangeState(State.Die);
            }            
                      
            return 0;
        }

        public override int PhysicUpdate()
        {           
                      
            if (!IsSpecialAttack)
            {
                if (Parameter.MoveDirection.sqrMagnitude > 0.001f)
                {
                    StateMachine.ChangeState(State.Move);
                    return 0;
                }
                else if (timeFrames >= GAMECONST.ANIM_IS_ATTACK_FRAMES)
                {
                    StateMachine.ChangeState(State.Idle);
                    return 0;
                }

                if (timeFrames == 12)
                {
                    Event.DealDamage(direction, Data.CharacterData.AttackRange);
                }
            }
            else
            {
                if (timeFrames >= GAMECONST.ANIM_IS_SPECIAL_ATTACK_FRAMES)
                {
                    StateMachine.ChangeState(State.Idle);
                    return 0;
                }

                if (timeFrames == 17) //Animation Attack
                {
                    Event.SpecialDealDamage(direction, Data.CharacterData.AttackRange, true);
                }
            }
            

            timeFrames++;
            return 0;
        }


        public override void Exit()
        {
            base.Exit();                     
            Event.SetTargetIndicatorPosition?.Invoke(null, false);
            if (!IsSpecialAttack)
            {
                Event.SetBool_Anim(GAMECONST.ANIM_IS_ATTACK, false);
            }
            else
            {
                Event.SetBool_Anim(GAMECONST.ANIM_IS_ULTI, false);
                Event.EndGiftBonus?.Invoke();
            }
            
        }

        

    }
}
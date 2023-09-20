using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MoveStopMove.Core.Character.LogicSystem
{
    using System;
    using WorldInterfaceSystem;
    using NavigationSystem;
    using PhysicSystem;
    using MoveStopMove.Core.Data;

    public class CharacterLogicSystem : AbstractCharacterSystem<AbstractLogicModule,LogicData,LogicParameter>
    {
        public LogicEvent Event;
        public CharacterLogicSystem(AbstractLogicModule module)
        {
            Data = ScriptableObject.CreateInstance(typeof(LogicData)) as LogicData;
            Parameter = ScriptableObject.CreateInstance(typeof(LogicParameter)) as LogicParameter;
            Event = ScriptableObject.CreateInstance(typeof(LogicEvent)) as LogicEvent;
            this.module = module;
            module.Initialize(Data,Parameter,Event);
        }


        public void SetCharacterInformation(CharacterData CharacterData, Transform PlayerTF)
        {
            Data.CharacterData = CharacterData;
            Parameter.PlayerTF = PlayerTF;
        }
        public void ReceiveInformation(WorldInterfaceData Data)
        {
            Parameter.TargetCharacter = Data.TargetCharacter;
            Parameter.IsGrounded = Data.IsGrounded;
            Parameter.IsHaveGround = Data.IsHaveGround;
            Parameter.Gift = Data.Gift;

            Event.CollideGift?.Invoke(Data.Gift);
            //Debug.Log("Logic:" + Data.CharacterPositions.Count);
        }

        public void ReceiveInformation(NavigationData Data)
        {
            Parameter.MoveDirection = Data.MoveDirection;
        }

        public void ReceiveInformation(PhysicData Data)
        {
            Parameter.Velocity = Data.Velocity;
        }

        public void ReceiveInformation(string code)
        {
            ((CharacterLogicModule)module).StateMachine.CurrentState.EventUpdate(null, code);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MoveStopMove.Core.Character.NavigationSystem
{
    using MoveStopMove.Core.Data;
    using WorldInterfaceSystem;
    public class CharacterNavigationSystem : AbstractCharacterSystem<AbstractNavigationModule,NavigationData,NavigationParameter>
    {
        public CharacterNavigationSystem(AbstractNavigationModule module)
        {
            Data = ScriptableObject.CreateInstance(typeof(NavigationData)) as NavigationData;
            Parameter = ScriptableObject.CreateInstance(typeof(NavigationParameter)) as NavigationParameter;
            this.module = module;
            module.Initialize(Data,Parameter);
        }

        public void ReceiveInformation(WorldInterfaceData Data)
        {
            Parameter.IsHaveGround = Data.IsHaveGround;
            Parameter.IsGrounded = Data.IsGrounded;
            Parameter.IsHaveObstances = Data.IsHaveObstances;
            Parameter.TargetCharacter = Data.TargetCharacter;
        }

        public void SetCharacterInformation(Transform Character,Transform SensorTF ,int PlayerInstanceID)
        {
            Parameter.CharacterTF = Character;
            Parameter.CharacterInstanceID = PlayerInstanceID;
            Parameter.SensorTF = SensorTF;
            
        }

        public void SetCharacterData(CharacterData CharacterData)
        {
            Parameter.CharacterData = CharacterData;
        }

        public void Reset()
        {
            Data.MoveDirection = Vector3.zero;
        }
    }
}
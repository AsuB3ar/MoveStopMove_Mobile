using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MoveStopMove.Core.Character.WorldInterfaceSystem
{
    using Data;
    public class CharacterWorldInterfaceSystem : AbstractCharacterSystem<WorldInterfaceModule,WorldInterfaceData,WorldInterfaceParameter>
    {  
        public CharacterWorldInterfaceSystem(WorldInterfaceModule module)
        {
            Data = ScriptableObject.CreateInstance(typeof(WorldInterfaceData)) as WorldInterfaceData;
            Parameter = ScriptableObject.CreateInstance(typeof(WorldInterfaceParameter)) as WorldInterfaceParameter;
            this.module = module;
            module.Initialize(Data,Parameter);
        }

        public void SetCharacterInformation(CharacterData CharacterData)
        {
            Parameter.CharacterData = CharacterData;
        }
    }
}
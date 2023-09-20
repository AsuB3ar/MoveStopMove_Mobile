using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MoveStopMove.Core.Character.LogicSystem
{
    using MoveStopMove.Core.Data;
    using Utilitys;
    public class LogicData : AbstractDataSystem<LogicData>
    {
        public CharacterData CharacterData;       
        protected override void UpdateDataClone()
        {
            if(Clone == null)
            {
                Clone = CreateInstance(typeof(LogicData)) as LogicData;
            }
        }

        

    }
}
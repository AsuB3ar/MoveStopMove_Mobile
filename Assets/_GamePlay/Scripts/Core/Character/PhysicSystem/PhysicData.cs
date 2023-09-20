using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.Core.Character.PhysicSystem
{
    public class PhysicData : AbstractDataSystem<PhysicData>
    {
        public Vector3 Velocity;

        protected override void UpdateDataClone()
        {
            if(Clone == null)
            {
                Clone = CreateInstance(typeof(PhysicData)) as PhysicData;
            }
            Clone.Velocity = Velocity;
        }
    }
}
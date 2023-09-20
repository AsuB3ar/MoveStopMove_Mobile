using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.Core.Character.WorldInterfaceSystem
{
    using Utilitys;
    public class WorldInterfaceData : AbstractDataSystem<WorldInterfaceData>
    {
        public bool IsHaveGround = false;
        public bool IsGrounded = false;
        public bool IsExitRoom = false;
        public bool IsHaveObstances = false;

        public BaseCharacter TargetCharacter;
        public Collider Gift;

        protected override void UpdateDataClone()
        {
            if(Clone == null)
            {
                Clone = CreateInstance(typeof(WorldInterfaceData)) as WorldInterfaceData;
            }
            Clone.IsHaveGround = IsHaveGround;
            Clone.IsGrounded = IsGrounded;
            Clone.IsExitRoom = IsExitRoom;
            Clone.IsHaveObstances = IsHaveObstances;

            Clone.TargetCharacter = TargetCharacter;
            Clone.Gift = Gift;
            //Clone.TargetCharacter = Cache.GetCacheList(Clone.TargetCharacter.GetHashCode(),TargetCharacter);
            //NOTE: Clone list EatBricks
        }
    }
}
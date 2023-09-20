using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.Core.Character.NavigationSystem
{
    using Utilitys;
    public class NavigationData : AbstractDataSystem<NavigationData>
    {
        private Vector3 moveDirection;
        public Vector3 MoveDirection
        {
            get => moveDirection;
            set
            {
                moveDirection = value;
            }
        }
        public bool Jump = false;

        protected override void UpdateDataClone()
        {
            if(Clone == null)
            {
                Clone = CreateInstance(typeof(NavigationData)) as NavigationData;
            }
            Clone.Jump = Jump;
            Clone.moveDirection = moveDirection;
        }
    }
}
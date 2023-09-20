using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.Core.Character.NavigationSystem
{
    public abstract class AbstractNavigationModule : AbstractModuleSystem<NavigationData,NavigationParameter>
    {
        protected NavigationData Data;
        protected NavigationParameter Parameter;
      
        public override void Initialize(NavigationData Data,NavigationParameter Parameter)
        {
            this.Data = Data;
            this.Parameter = Parameter;
        }

        public virtual void Reset()
        {
            //Reset Data;
        }
    }
}
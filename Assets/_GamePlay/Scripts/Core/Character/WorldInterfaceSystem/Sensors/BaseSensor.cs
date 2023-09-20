using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.Core.Character.WorldInterfaceSystem
{
    public enum SensorType
    {
        Detect = 0,
        Vision = 1
    }
    public abstract class BaseSensor : MonoBehaviour
    {
        protected WorldInterfaceData Data;
        protected WorldInterfaceParameter Parameter;
        [SerializeField]
        protected LayerMask layer;
        public void Initialize( WorldInterfaceData Data, WorldInterfaceParameter Parameter)
        {
            this.Parameter = Parameter;
            this.Data = Data;
        }
        public abstract void UpdateData();
        
    }
}
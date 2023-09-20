using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.Core.Character.WorldInterfaceSystem
{
    public class WorldInterfaceModule : AbstractModuleSystem<WorldInterfaceData,WorldInterfaceParameter>
    {
        WorldInterfaceData Data;
        WorldInterfaceParameter Parameter;
        [SerializeField]
        List<BaseSensor> sensors;
        public override void Initialize(WorldInterfaceData Data,WorldInterfaceParameter Parameter)
        {
            this.Data = Data;
            this.Parameter = Parameter;
            for(int i = 0; i < sensors.Count; i++)
            {
                sensors[i].Initialize(this.Data,this.Parameter);
            }
        }
        public override void UpdateData()
        {
            for (int i = 0; i < sensors.Count; i++)
            {
                sensors[i].UpdateData();
            }
        }
    }
}
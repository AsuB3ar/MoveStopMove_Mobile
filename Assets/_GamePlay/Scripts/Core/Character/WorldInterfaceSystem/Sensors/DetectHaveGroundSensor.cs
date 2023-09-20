using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.Core.Character.WorldInterfaceSystem
{
    public class DetectHaveGroundSensor : BaseSensor
    {
        [SerializeField]
        Transform checkHaveGroundPoint;
        [SerializeField]
        float distance;
        Ray ray = new Ray(Vector3.zero,Vector3.down);
        public override void UpdateData()
        {
            ray.origin = checkHaveGroundPoint.position;
            Data.IsHaveGround = Physics.Raycast(ray, distance, layer);
        }

        private void OnDrawGizmos()
        {
            if(checkHaveGroundPoint != null)
            {
                Gizmos.DrawLine(checkHaveGroundPoint.position, checkHaveGroundPoint.position + Vector3.down * distance);
            }
            
        }
    }
}
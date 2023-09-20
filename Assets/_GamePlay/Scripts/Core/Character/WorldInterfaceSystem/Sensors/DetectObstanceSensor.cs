using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.Core.Character.WorldInterfaceSystem
{
    public class DetectObstanceSensor : BaseSensor
    {
        [SerializeField]
        Transform checkPoint;
        [SerializeField]
        float distance;

        Ray ray = new Ray();
        public override void UpdateData()
        {
            ray.origin = checkPoint.position;
            ray.direction = checkPoint.forward;
            Data.IsHaveObstances = Physics.Raycast(ray, distance, layer);
        }

        private void OnDrawGizmos()
        {
            if (checkPoint != null)
            {
                Gizmos.DrawLine(checkPoint.position, checkPoint.position + checkPoint.forward * distance);
            }

        }
    }
}
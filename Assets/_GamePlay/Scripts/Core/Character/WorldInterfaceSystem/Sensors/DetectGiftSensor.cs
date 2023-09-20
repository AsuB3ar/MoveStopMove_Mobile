using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MoveStopMove.Core.Character.WorldInterfaceSystem
{
    public class DetectGiftSensor : BaseSensor
    {
        public readonly Vector3 checkRadiusUnit = new Vector3(0.3f, 0.6f, 0.3f);

        [SerializeField]
        Transform checkPoint;       
        Vector3 lastCheckRadius;
        Collider[] gifts = new Collider[1];
        int giftCount;
        public override void UpdateData()
        {
            gifts[0] = null;
            lastCheckRadius = checkRadiusUnit * Parameter.CharacterData.Size;
            giftCount = Physics.OverlapBoxNonAlloc(checkPoint.transform.position, lastCheckRadius, gifts, Quaternion.identity, layer);
            Data.Gift = gifts[0];
        }

        private void OnDrawGizmos()
        {
            if (checkPoint != null)
            {
                Gizmos.DrawCube(checkPoint.position, lastCheckRadius * 2);
            }
        }
    }
}
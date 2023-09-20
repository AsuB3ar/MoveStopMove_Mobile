using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.Core.Character.WorldInterfaceSystem
{
    public class DetectGroundSensor : BaseSensor
    {
        public Transform groundCheck;
        public float distance = 0.4f;
        public override void UpdateData()
        {
            Data.IsGrounded = Physics.CheckSphere(groundCheck.position, distance, layer);
            //Debug.Log("WorldInterface: " + Data.IsGrounded);
        }

        private void OnDrawGizmos()
        {
            if(groundCheck != null)
            {
                Gizmos.DrawSphere(groundCheck.position, distance);
            }     
            
        }
    }
}
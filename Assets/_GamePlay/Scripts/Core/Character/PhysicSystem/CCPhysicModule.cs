using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.Core.Character.PhysicSystem {
    public class CCPhysicModule : AbstractPhysicModule
    {
        public CharacterController controller;
        [SerializeField]
        GameObject charModel;
        [SerializeField]
        GameObject charSensor;     
        [SerializeField]
        bool active;
        [SerializeField]
        bool useGravity;
        [SerializeField]
        float rotateSpeed = 10f;
        

        Quaternion rotGoal;
        private void Awake()
        {
            SetActive(active);
        }
        public override void SetVelocity(Vector3 velocity)
        {
            controller.Move(velocity * Time.deltaTime);
            Data.Velocity = velocity;
        }
        public override void OnInit()
        {
            SetActive(true);
        }

        public override void SetRotation(GAMECONST.PHYSIC_OTYPE type,Quaternion rotation)
        {
            controller.enabled = false;
            if(type == GAMECONST.PHYSIC_OTYPE.Character)
            {
                gameObject.transform.rotation = rotation;
            }
            else if(type == GAMECONST.PHYSIC_OTYPE.Model)
            {
                charModel.transform.rotation = rotation;
            }
            else if(type == GAMECONST.PHYSIC_OTYPE.Sensor)
            {
                charSensor.transform.rotation = rotation;
            }
            controller.enabled = true;
        }

        public override void SetSmoothRotation(GAMECONST.PHYSIC_OTYPE type, Vector3 direction)
        {
            if (type == GAMECONST.PHYSIC_OTYPE.Character)
            {
                rotGoal = Quaternion.LookRotation(direction);
                gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, rotGoal, rotateSpeed * Time.deltaTime);
            }
            else if (type == GAMECONST.PHYSIC_OTYPE.Model)
            {
                rotGoal = Quaternion.LookRotation(direction);
                charModel.transform.rotation = Quaternion.Slerp(charModel.transform.rotation, rotGoal, rotateSpeed * Time.deltaTime);
            }
            else if (type == GAMECONST.PHYSIC_OTYPE.Sensor)
            {
                rotGoal = Quaternion.LookRotation(direction);
                charSensor.transform.rotation = Quaternion.Slerp(charSensor.transform.rotation, rotGoal, rotateSpeed * Time.deltaTime);
            }

        }

        public override void SetScale(GAMECONST.PHYSIC_OTYPE type, Vector3 scale)
        {
            if (type == GAMECONST.PHYSIC_OTYPE.Character)
            {
                gameObject.transform.localScale = scale;
            }
            else if (type == GAMECONST.PHYSIC_OTYPE.Model)
            {
                charModel.transform.localScale = scale;
            }
            else if (type == GAMECONST.PHYSIC_OTYPE.Sensor)
            {
                charSensor.transform.localScale = scale;
            }
        }

        public override void SetScale(GAMECONST.PHYSIC_OTYPE type, float ratio)
        {
            if (type == GAMECONST.PHYSIC_OTYPE.Character)
            {
                gameObject.transform.localScale = gameObject.transform.localScale * ratio;
            }
            else if (type == GAMECONST.PHYSIC_OTYPE.Model)
            {
                charModel.transform.localScale = charModel.transform.localScale = gameObject.transform.localScale * ratio;

            }
            else if (type == GAMECONST.PHYSIC_OTYPE.Sensor)
            {
                charSensor.transform.localScale = charSensor.transform.localScale = gameObject.transform.localScale * ratio;
            }
        }
        public override void SetActive(bool value)
        {
            active = value;
            controller.enabled = value;
            controller.detectCollisions = value;
        }
        public override void UpdateData()
        {
            if (useGravity)
            {
                if (Parameter.GravityParameter < 0.001f) return;
                else
                {
                    Data.Velocity.y += Parameter.GRAVITY * Parameter.GravityParameter * Time.deltaTime;
                }
            }
            
        }

        
    }
}

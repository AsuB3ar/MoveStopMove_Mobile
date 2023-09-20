
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MoveStopMove.Core.Character.NavigationSystem
{
    using MoveStopMove.Core.Data;
    public class NavigationParameter : AbstractParameterSystem
    {
        public Transform CharacterTF;
        public Transform SensorTF;
        public CharacterData CharacterData;
        public int CharacterInstanceID;

        public bool IsGrounded = false;
        public bool IsHaveGround = false;
        public bool IsHaveObstances = false;

        public BaseCharacter TargetCharacter;
    }
}
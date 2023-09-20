using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.Core.Data
{
    [CreateAssetMenu(fileName = "newItemData", menuName = "Data/Level")]
    public class LevelData : ScriptableObject
    {

        public float Size;
        public List<Vector3> ObstancePositions;
        public List<Vector3> GiftPositions;
        public int numOfPlayers = 50;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.ContentCreation
{
    public enum ItemState
    {
        Lock = 0,
        Unlock = 1,
        Selected = 2
    }
    [CreateAssetMenu(fileName = "newItemData", menuName = "Data/Item")] 
    public class ItemData : ScriptableObject
    {
        [Header("Static Data")]
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;

        public Sprite icon;
        public PoolID poolID;
        public PantSkin pant;
        public UIItemType type;
        public int price;

        [Header("Dynamic Data")]
        public ItemState state;
    }
}
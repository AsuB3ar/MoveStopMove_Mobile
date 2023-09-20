using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.ContentCreation
{
    using System;
    using MoveStopMove.Manager;
    public class Gift : MonoBehaviour,IDespawn
    {
        public event Action<Gift> OnGiftDespawn;
        [SerializeField]
        PoolID poolID;
        public void OnDespawn()
        {
            PrefabManager.Inst.PushToPool(gameObject, poolID);
            OnGiftDespawn?.Invoke(this);
        }
 
    }
}
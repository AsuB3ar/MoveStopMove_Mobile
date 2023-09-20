using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.Core.Character
{
    using System;
    public abstract class AbstractDataSystem<D> : ScriptableObject
    {
        protected D Clone;
        public D OnUpdateData()
        {
            UpdateDataClone();
            return Clone;
        }

        protected abstract void UpdateDataClone();
    }
}

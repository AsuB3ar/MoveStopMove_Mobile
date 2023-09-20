using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Utilitys
{
    public class CheckCollide : MonoBehaviour
    {
        public event Action<Collider> OnColliderEnter;
        private void OnTriggerEnter(Collider other)
        {

            OnColliderEnter?.Invoke(other);
        }
    }
}
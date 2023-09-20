using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Utilitys
{
    public class VisualEffectController : MonoBehaviour
    {
        [SerializeField]
        private VisualEffect effect;

        public void Init(Transform parent, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            gameObject.transform.parent = parent;
            gameObject.transform.localPosition = position;
            gameObject.transform.localRotation = rotation;
            gameObject.transform.localScale = scale;
            Stop();
        }
        public void SetColor(Color color)
        {
            effect.SetVector4("HitColor", color);
        }


        public void Play()
        {
            //effect.SetVector4()
            gameObject.SetActive(true);
            effect.Reinit();
            effect.Play();
        }

        public void Stop()
        {
            effect.Stop();
            gameObject.SetActive(false);
        }
    }
}
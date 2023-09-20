using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilitys
{
    public static class Utils
    {
        public static Vector3 ScreenToWorld(Camera camera, Vector3 position)
        {
            position.z = camera.nearClipPlane;
            return camera.ScreenToWorldPoint(position);           
        }
    }
}
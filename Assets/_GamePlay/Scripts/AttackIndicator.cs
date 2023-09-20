using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIndicator : MonoBehaviour
{
    private readonly Vector3 INIT_INDICATOR_SCALE = Vector3.one;
    private const float speed = 3f;

    Vector3 targetScale;
    bool isReachScale = true;
    void FixedUpdate()
    {
        if (!isReachScale)
        {
            Scale();
        }
    }

    private void Scale()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, speed * Time.fixedDeltaTime);
        if((transform.localScale - targetScale).sqrMagnitude < 0.000001f)
        {
            isReachScale = true;
        }
    }

    public void ScaleUp(float parameter)
    {
        isReachScale = false;
        targetScale = INIT_INDICATOR_SCALE * parameter;
    }
}

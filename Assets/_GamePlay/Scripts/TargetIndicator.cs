using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float rotationSpeed;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0, rotationSpeed * Time.fixedDeltaTime * 60, 0, Space.Self);
    }
}

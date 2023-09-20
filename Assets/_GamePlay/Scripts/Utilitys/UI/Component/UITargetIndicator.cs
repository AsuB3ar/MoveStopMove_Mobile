using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITargetIndicator : MonoBehaviour
{
    public const float WIDTH = 100;
    public const float HEIGHT = 100;
    private const float DISTANCE = 80;

    [SerializeField]
    TMP_Text textLevel;
    [SerializeField]
    Image image;
    [SerializeField]
    Transform directionTF;
    [SerializeField]
    Image directionImage;
    Vector3 oldPos;
    public void SetLevel(int level)
    {
        textLevel.text = level.ToString();
    }

    public void SetColor(Color color)
    {
        image.color = color;
        directionImage.color = color;
    }

    public void SetActiveDirection(bool value)
    {
        directionTF.gameObject.SetActive(value);        
    }
    public void SetDirection()
    {
        //NOTE: Direction Vector must be normalize
        Vector2 direction = new Vector2(transform.localPosition.x, transform.localPosition.y).normalized;
        float angle = Vector2.SignedAngle(Vector2.up, direction);
        directionTF.localRotation = Quaternion.Euler(0, 0, angle);
        directionTF.localPosition = new Vector3(DISTANCE * direction.x, DISTANCE * direction.y, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum CameraPosition
{
    MainMenu = 0,
    Gameplay = 1,
    ShopSkin = 2,
    ShopWeapon = 3,
}
[DefaultExecutionOrder(-1)]
public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    public const float SOFT_ZONE_HEIGHT_INIT = 0.05f;
    public readonly Vector3 MainMenuPosition = new Vector3(0, 1.5f, 3);
    public readonly Vector3 MainMenuRotation = new Vector3(15, 175, 0);

    public readonly Vector3 GameplayPosition = new Vector3(0.05f, 8.9f, -6.914f);
    public readonly Vector3 GameplayRotation = new Vector3(45, 0, 0);

    public readonly Vector3 ShopWeaponPosition = new Vector3(0, 1.26f, 1.79f);
    public readonly Vector3 ShopWeaponRotation = new Vector3(15, 180, 0);

    public readonly Vector3 ShopSkinPosition = new Vector3(0.05f, 2.44f, 3.43f);
    public readonly Vector3 ShopSkinRotation = new Vector3(30, 180, 0);
    [SerializeField]
    CinemachineVirtualCamera virtualCamera;
    CinemachineFramingTransposer comp;

    private float speed = 2f;
    private Vector3 targetPos;
    private Vector3 targetRot;

    private bool IsReachDestination = true;
    private bool IsReachRotation = true;
    float rate = 0;

    private void Awake()
    {
        comp = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsReachDestination)
        {
            Move();
        }
        if (!IsReachRotation)
        {
            Rotate();
        }
    }

    private void Move()
    {
        Vector3 newPos = Vector3.Lerp(comp.m_TrackedObjectOffset, targetPos, speed * Time.fixedDeltaTime);
        if((comp.m_TrackedObjectOffset - newPos).sqrMagnitude < 0.0000001f)
        {
            IsReachDestination = true;
        }
        comp.m_TrackedObjectOffset = newPos;
    }
    
    private void Rotate()
    {
        Vector3 newRot = Vector3.Lerp(transform.rotation.eulerAngles, targetRot, speed * Time.fixedDeltaTime);
        if((transform.localRotation.eulerAngles - newRot).sqrMagnitude < 0.0000001f)
        {
            IsReachRotation = true;
        }
        transform.localRotation = Quaternion.Euler(newRot);
    }

    public void MoveTo(CameraPosition position)
    {
        

        if(position == CameraPosition.MainMenu)
        {
            targetPos = MainMenuPosition;
            targetRot = MainMenuRotation;
            speed = 2;
        }
        else if(position == CameraPosition.Gameplay)
        {
            targetPos = GameplayPosition;
            targetRot = GameplayRotation;
            speed = 8;
        }
        else if(position == CameraPosition.ShopSkin)
        {
            targetPos = ShopSkinPosition;
            targetRot = ShopSkinRotation;
            speed = 4;
        }
        else if(position == CameraPosition.ShopWeapon)
        {
            targetPos = ShopWeaponPosition;
            targetRot = ShopWeaponRotation;
            speed = 4;
        }
        IsReachDestination = false;
        IsReachRotation = false;
    }

    public void MoveTo(float size)
    {
        targetPos = GameplayPosition;
        targetPos.y *= size;
        targetPos.z *= size;

        speed = 10;
        IsReachDestination = false;
        comp.m_SoftZoneHeight = SOFT_ZONE_HEIGHT_INIT * size;
    }
    public void Follow(Transform followObject)
    {
        virtualCamera.Follow = followObject;
    }
}